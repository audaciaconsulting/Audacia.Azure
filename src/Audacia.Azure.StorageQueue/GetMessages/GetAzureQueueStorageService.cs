using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        /// <param name="queueClient">Queue client used get message from Azure queue storage.</param>
        public GetAzureQueueStorageService(QueueClient queueClient) : base(queueClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="QueueStorageOption"/>.
        /// </summary>
        /// <param name="queueStorageConfig">Config option used to create <see cref="QueueClient"/>.</param>
        public GetAzureQueueStorageService(IOptions<QueueStorageOption> queueStorageConfig) : base(queueStorageConfig)
        {
        }

        /// <summary>
        /// Returns the top message from a queue.
        /// </summary>
        /// <param name="command">
        /// Containing information of the queue name and whether to delete the message from storage after it has been received.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The message from the queue if there is one.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        public async Task<AzureQueueStorageMessage?> GetAsync(GetMessageStorageQueueCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            await PreQueueChecksAsync(command.QueueName, cancellationToken).ConfigureAwait(false);

            var queueProperties = await QueueClient.GetPropertiesAsync(cancellationToken).ConfigureAwait(false);
            if (queueProperties.Value.ApproximateMessagesCount > 0)
            {
                var nextMessage = await QueueClient.ReceiveMessageAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

                if (command.ShouldDeleteMessageAfterReceiving)
                {
                    await DeleteMessageAsync(nextMessage.Value, cancellationToken).ConfigureAwait(false);
                }

                return new AzureQueueStorageMessage(
                    nextMessage.Value.MessageId,
                    nextMessage.Value.PopReceipt,
                    nextMessage.Value.MessageText,
                    nextMessage.Value.InsertedOn,
                    DateTime.Now);
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
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A collection of size <paramref name="command.AmountToReceive"/> of <see cref="AzureQueueStorageMessage"/>'s if
        /// available if not the remaining amount that are currently in the queue.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        public async Task<IEnumerable<AzureQueueStorageMessage>> GetSomeAsync(GetMessagesStorageQueueCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            await PreQueueChecksAsync(command.QueueName, cancellationToken).ConfigureAwait(false);

            var queueProperties = await QueueClient.GetPropertiesAsync(cancellationToken).ConfigureAwait(false);
            if (queueProperties.Value.ApproximateMessagesCount > 0)
            {
                var response = await QueueClient.ReceiveMessagesAsync(command.AmountToReceive, cancellationToken: cancellationToken).ConfigureAwait(false);
                var nextMessages = response.Value;

                await ProcessDeletingMessageAsync(command, nextMessages, cancellationToken).ConfigureAwait(false);

                return nextMessages.Select(message => new AzureQueueStorageMessage(
                        message.MessageId,
                        message.PopReceipt,
                        message.MessageText,
                        message.InsertedOn,
                        DateTime.Now))
                    .ToList();
            }

            return new List<AzureQueueStorageMessage>();
        }

        private async Task ProcessDeletingMessageAsync(
            GetMessagesStorageQueueCommand command,
            IEnumerable<QueueMessage> nextMessages,
            CancellationToken cancellationToken)
        {
            if (command.ShouldDeleteMessageAfterReceiving)
            {
                foreach (var nextMessage in nextMessages)
                {
                    await DeleteMessageAsync(nextMessage, cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}