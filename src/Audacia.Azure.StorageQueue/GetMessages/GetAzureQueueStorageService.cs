using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audacia.Azure.StorageQueue.Common.Services;
using Audacia.Azure.StorageQueue.Config;
using Audacia.Azure.StorageQueue.GetMessages.Commands;
using Audacia.Azure.StorageQueue.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.StorageQueue.GetMessages
{
    /// <summary>
    /// Get service for returning messages from an Azure Queue Storage account.
    /// </summary>
    public class GetAzureQueueStorageService : BaseQueueStorageService, IGetAzureQueueStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="QueueClient"/> has being added to the DI.
        /// </summary>
        /// <param name="queueClient"></param>
        /// <param name="formatProvider"></param>
        protected GetAzureQueueStorageService(QueueClient queueClient, IFormatProvider formatProvider) : base(
            queueClient, formatProvider)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="QueueStorageOption"/>.
        /// </summary>
        /// <param name="queueStorageConfig"></param>
        /// <param name="formatProvider"></param>
        protected GetAzureQueueStorageService(
            IOptions<QueueStorageOption> queueStorageConfig, IFormatProvider formatProvider) : base(
            queueStorageConfig, formatProvider)
        {
        }

        /// <summary>
        /// Returns the top message from a queue.
        /// </summary>
        /// <param name="command">
        /// Containing information of the queue name and whether to delete the message from storage after it has been received.
        /// </param>
        /// <returns>The message from the queue if there is one.</returns>
        public async Task<AzureQueueStorageMessage> GetAsync(GetMessageStorageQueueCommand command)
        {
            await PreQueueChecksAsync(command.QueueName);

            var queueProperties = await QueueClient.GetPropertiesAsync();
            if (queueProperties.Value.ApproximateMessagesCount > 0)
            {
                var nextMessage = await QueueClient.ReceiveMessageAsync();

                if (command.DeleteMessageAfterReceiving)
                {
                    await DeleteMessageAsync(nextMessage.Value);
                }

                if (nextMessage.Value.InsertedOn != null)
                {
                    return new AzureQueueStorageMessage(
                        nextMessage.Value.MessageId,
                        nextMessage.Value.PopReceipt,
                        nextMessage.Value.MessageText,
                        nextMessage.Value.InsertedOn.Value.DateTime,
                        DateTime.Now);
                }
            }

            return null;
        }

        /// <summary>
        /// Return a custom amount of messages from a queue.
        /// </summary>
        /// <param name="command">
        /// Command containing the queue name, the amount of messages to get and whether to delete the messages after
        /// they have been pulled from storage.
        /// </param>
        /// <returns>
        /// A collection of size <paramref name="command.AmountToReceive"/> of <see cref="AzureQueueStorageMessage"/>'s if
        /// available if not the remaining amount that are currently in the queue.
        /// </returns>
        public async Task<IEnumerable<AzureQueueStorageMessage>> GetSomeAsync(GetMessagesStorageQueueCommand command)
        {
            await PreQueueChecksAsync(command.QueueName);

            var queueProperties = await QueueClient.GetPropertiesAsync();
            if (queueProperties.Value.ApproximateMessagesCount > 0)
            {
                var response = await QueueClient.ReceiveMessagesAsync(command.AmountToReceive);
                var nextMessages = response.Value;

                var allMessagesHaveInsertedOn = await ProcessDeletingMessageAsync(command, nextMessages);
                if (allMessagesHaveInsertedOn)
                {
                    return nextMessages.Select(message => new AzureQueueStorageMessage(
                            message.MessageId,
                            message.PopReceipt,
                            message.MessageText,
                            message.InsertedOn.Value.DateTime,
                            DateTime.Now))
                        .ToList();
                }
            }

            return new List<AzureQueueStorageMessage>();
        }

        private async Task<bool> ProcessDeletingMessageAsync(
            GetMessagesStorageQueueCommand command,
            IEnumerable<QueueMessage> nextMessages)
        {
            if (command.DeleteMessageAfterReceiving)
            {
                foreach (var nextMessage in nextMessages)
                {
                    await DeleteMessageAsync(nextMessage);
                }
            }

            var allMessagesHaveInsertedOn = true;
            foreach (var message in nextMessages)
            {
                if (!(message.InsertedOn is { } offset))
                {
                    allMessagesHaveInsertedOn = false;
                    break;
                }
            }

            return allMessagesHaveInsertedOn;
        }
    }
}
