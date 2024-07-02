using Audacia.Azure.StorageQueue.Common.Services;
using Audacia.Azure.StorageQueue.Config;
using Audacia.Azure.StorageQueue.Extensions;
using Azure.Storage.Queues;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.StorageQueue.DeleteMessageFromQueue
{
    /// <summary>
    /// Delete service for removing messages from an Azure Queue Storage account.
    /// </summary>
    public class DeleteAzureQueueStorageService : BaseQueueStorageService, IDeleteAzureQueueStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="QueueClient"/> has being added to the DI.
        /// </summary>
        /// <param name="queueClient">Queue client used get message from Azure queue storage.</param>
        public DeleteAzureQueueStorageService(QueueClient queueClient) : base(
            queueClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="QueueStorageOption"/>.
        /// </summary>
        /// <param name="queueStorageConfig">Config options used to create an instance of <see cref="QueueClient"/>.</param>
        public DeleteAzureQueueStorageService(IOptions<QueueStorageOption> queueStorageConfig) : base(
            queueStorageConfig)
        {
        }

        /// <summary>
        /// Removing a message from a queue.
        /// </summary>
        /// <param name="queueName">The name of the queue you want to remove a message from.</param>
        /// <param name="messageId">Id of the message which you want to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Bool whether the message was deleted from the queue.</returns>
        public async Task<bool> ExecuteAsync(string queueName, string messageId, CancellationToken cancellationToken = default)
        {
            await PreQueueChecksAsync(queueName, cancellationToken).ConfigureAwait(false);

            var queueMessages = await QueueClient.ReceiveMessagesAsync(32, cancellationToken: cancellationToken).ConfigureAwait(false);

            var peekedMessages = queueMessages.Value;

            var peekMessage = peekedMessages.WithMessageId(messageId).FirstOrDefault();

            if (peekMessage != null)
            {
                using var deleteResponse = await QueueClient.DeleteMessageAsync(messageId, peekMessage.PopReceipt, cancellationToken).ConfigureAwait(false);

                return deleteResponse.Status == 204;
            }

            return false;
        }
    }
}
