﻿using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues.Models;

namespace Audacia.Azure.StorageQueue.AddMessageToQueue
{
    /// <summary>
    /// Add service for adding messages to an Azure Queue Storage account.
    /// </summary>
    public interface IAddAzureQueueStorageService
    {
        /// <summary>
        /// Add a new message to a queue.
        /// </summary>
        /// <param name="queueName">Name of the queue which the message will be added too.</param>
        /// <param name="queueMessage">String containing the context of the message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The receipt from adding a message to the queue.</returns>
        Task<SendReceipt> ExecuteAsync(string queueName, string queueMessage, CancellationToken cancellationToken = default);
    }
}