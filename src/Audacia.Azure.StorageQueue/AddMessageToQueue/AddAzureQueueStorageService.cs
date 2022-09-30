using Audacia.Azure.StorageQueue.Common.Services;
using Audacia.Azure.StorageQueue.Config;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.StorageQueue.AddMessageToQueue
{
    /// <summary>
    /// Add service for adding messages to an Azure Queue Storage account.
    /// </summary>
    public class AddAzureQueueStorageService : BaseQueueStorageService, IAddAzureQueueStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="QueueClient"/> has being added to the DI.
        /// </summary>
        /// <param name="queueClient">Queue client used get message from Azure queue storage.</param>
        public AddAzureQueueStorageService(QueueClient queueClient) : base(queueClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="QueueStorageOption"/>.
        /// </summary>
        /// <param name="queueStorageConfig">Config option containing information on how to create <see cref="QueueClient"/>.</param>
        public AddAzureQueueStorageService(IOptions<QueueStorageOption> queueStorageConfig) : base(queueStorageConfig)
        {
        }

        /// <summary>
        /// Add a new message to a queue.
        /// </summary>
        /// <param name="queueName">Name of the queue which the message will be added too.</param>
        /// <param name="queueMessage">String containing the context of the message.</param>
        /// <returns>The receipt from adding a message to the queue.</returns>
        public async Task<SendReceipt> ExecuteAsync(string queueName, string queueMessage)
        {
            await PreQueueChecksAsync(queueName).ConfigureAwait(false);

            var response = await QueueClient.SendMessageAsync(queueMessage).ConfigureAwait(false);

            return response.Value;
        }
    }
}
