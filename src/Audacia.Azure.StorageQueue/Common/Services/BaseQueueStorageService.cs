﻿using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Azure.StorageQueue.Config;
using Audacia.Azure.StorageQueue.Exceptions;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.StorageQueue.Common.Services
{
    /// <summary>
    /// Base service for Azure queue storage.
    /// </summary>
    public class BaseQueueStorageService
    {
        private readonly string _storageAccountConnectionString =
            "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};QueueEndpoint={2};";

        private readonly string _storageAccountUrl = "https://{0}.queue.core.windows.net";

        private readonly string _accountName;

        /// <summary>
        /// Gets or sets the instance of the QueueClient which connect to the Queue in the Azure Storage Account.
        /// </summary>
        protected QueueClient QueueClient { get; set; } = default!;

        /// <summary>
        /// Gets the format provider for formatting exception messages.
        /// </summary>
        protected static IFormatProvider FormatProvider => CultureInfo.InvariantCulture;

        private string StorageAccountString =>
            string.Format(FormatProvider, _storageAccountUrl, _accountName);

        /// <summary>
        ///  Gets the Url of the Azure Storage account.
        /// </summary>
        protected Uri StorageAccountUrl => new Uri(StorageAccountString);

        /// <summary>
        ///  Gets the connection string used to connect to the Azure Storage account.
        /// </summary>
        public string StorageAccountConnectionString { get; } = default!;

        /// <summary>
        /// Client variation of the Constructor.
        /// </summary>
        /// <param name="queueClient"><see cref="QueueClient"/> from IoC.</param>
        /// <exception cref="StorageQueueConfigurationException">If <see cref="QueueClient"/> has not been configured.</exception>
        protected BaseQueueStorageService(QueueClient queueClient)
        {
            QueueClient = queueClient ?? throw StorageQueueConfigurationException.QueueClientNotConfigured();
            _accountName = queueClient.AccountName;
        }

        /// <summary>
        /// Options variation of the Constructor.
        /// </summary>
        /// <param name="queueStorageConfig">Options of <see cref="QueueStorageOption"/>.</param>
        /// <exception cref="StorageQueueConfigurationException">
        /// Exception if there is a miss configuration with <see cref="QueueStorageOption"/>.
        /// </exception>
        protected BaseQueueStorageService(IOptions<QueueStorageOption> queueStorageConfig)
        {
            OptionsConfigCheck(queueStorageConfig: queueStorageConfig);

            _accountName = queueStorageConfig.Value.AccountName;

            var queueEndpoint = queueStorageConfig?.Value?.QueueEndpoint?.ToString();
            if (!string.IsNullOrEmpty(queueEndpoint))
            {
                _storageAccountUrl = queueEndpoint;
            }

            StorageAccountConnectionString = string.Format(
                FormatProvider,
                _storageAccountConnectionString,
                queueStorageConfig!.Value.AccountName,
                queueStorageConfig!.Value.AccountKey,
                StorageAccountString);
        }

        /// <summary>
        /// Checks if the <paramref name="queueStorageConfig"/> are valid to create a <see cref="QueueClient"/>.
        /// </summary>
        /// <param name="queueStorageConfig">An instance of the <see cref="QueueStorageOption"/>.</param>
        /// <exception cref="StorageQueueConfigurationException">
        /// Exception thrown when a property from the <see cref="QueueStorageOption"/> is either null or empty.
        /// </exception>
        private static void OptionsConfigCheck(IOptions<QueueStorageOption> queueStorageConfig)
        {
            if (queueStorageConfig?.Value == null)
            {
                throw StorageQueueConfigurationException.OptionsNotConfigured();
            }

            if (string.IsNullOrEmpty(queueStorageConfig.Value.AccountName))
            {
                throw StorageQueueConfigurationException.AccountNameNotConfigured(FormatProvider);
            }

            if (string.IsNullOrEmpty(queueStorageConfig.Value.AccountKey))
            {
                throw StorageQueueConfigurationException.AccountKeyNotConfigured(FormatProvider);
            }
        }

        /// <summary>
        /// Checks on the queue before any actions are done.
        /// </summary>
        /// <param name="queueName">Name of the queue you are checking.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="QueueDoesNotExistException">
        /// If the queue you are wanting to do something with, does not exist.
        /// </exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task PreQueueChecksAsync(string queueName, CancellationToken cancellationToken)
        {
            QueueClient = new QueueClient(StorageAccountConnectionString, queueName);

            var queueExists = await QueueClient.ExistsAsync(cancellationToken).ConfigureAwait(false);

            if (!queueExists)
            {
                throw new QueueDoesNotExistException(queueName, FormatProvider);
            }
        }

        /// <summary>
        /// Deletes a message from a <see cref="QueueClient"/>.
        /// </summary>
        /// <param name="message"><see cref="QueueMessage"/> to be removed from the <see cref="QueueClient"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Whether the message has been removed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is null.</exception>
        protected async Task<bool> DeleteMessageAsync(QueueMessage message, CancellationToken cancellationToken)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var receivedMessageId = message.MessageId;
            var receivedMessagePopReceipt = message.PopReceipt;

            using var response = await QueueClient.DeleteMessageAsync(receivedMessageId, receivedMessagePopReceipt, cancellationToken)
                .ConfigureAwait(false);

            return response.Status == 200; // might be 202 as might not execute
        }
    }
}