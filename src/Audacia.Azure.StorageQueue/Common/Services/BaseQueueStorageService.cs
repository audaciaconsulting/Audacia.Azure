﻿using System;
using System.Globalization;
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
            "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}";

        private readonly string _storageAccountUrl = "https://{0}.queue.core.windows.net";

        private readonly string _accountName;

        protected QueueClient QueueClient { get; set; }

        protected IFormatProvider FormatProvider { get; }

        protected Uri StorageAccountUrl => new(string.Format(FormatProvider, _storageAccountUrl, _accountName));

        public string StorageAccountConnectionString { get; }

        /// <summary>
        /// Client variation of the Constructor.
        /// </summary>
        /// <param name="queueClient"><see cref="QueueClient"/> from Ioc</param>
        /// <param name="formatProvider"></param>
        /// <exception cref="StorageQueueConfigurationException">If <see cref="QueueClient"/> has not been configured.</exception>
        protected BaseQueueStorageService(QueueClient queueClient, IFormatProvider formatProvider)
        {
            QueueClient = queueClient ?? throw StorageQueueConfigurationException.QueueClientNotConfigured();
            FormatProvider = formatProvider;
            _accountName = queueClient.AccountName;
        }

        /// <summary>
        /// Options variation of the Constructor.
        /// </summary>
        /// <param name="queueStorageConfig">Options of <see cref="QueueStorageOption"/>.</param>
        /// <param name="formatProvider"></param>
        /// <exception cref="StorageQueueConfigurationException">
        /// Exception if there is a miss configuration with <see cref="QueueStorageOption"/>.
        /// </exception>
        protected BaseQueueStorageService(
            IOptions<QueueStorageOption> queueStorageConfig,
            IFormatProvider formatProvider)
        {
            FormatProvider = formatProvider ?? CultureInfo.InvariantCulture;

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

            _accountName = queueStorageConfig.Value.AccountName;

            StorageAccountConnectionString = string.Format(FormatProvider, _storageAccountConnectionString,
                queueStorageConfig.Value.AccountName, queueStorageConfig.Value.AccountKey);
        }

        /// <summary>
        /// Checks on the queue before any actions are done.
        /// </summary>
        /// <param name="queueName">Name of the queue you are checking</param>
        /// <exception cref="QueueDoesNotExistException">
        /// If the queue you are wanting to do something with, does not exist.
        /// </exception>
        protected async Task PreQueueChecksAsync(string queueName)
        {
            QueueClient = new QueueClient(StorageAccountConnectionString, queueName);

            var queueExists = await QueueClient.ExistsAsync();

            if (!queueExists)
            {
                throw new QueueDoesNotExistException(queueName, FormatProvider);
            }
        }

        /// <summary>
        /// Deletes a message from a <see cref="QueueClient"/>.
        /// </summary>
        /// <param name="message"><see cref="QueueMessage"/> to be removed from the <see cref="QueueClient"/>.</param>
        /// <returns>Whether the message has been removed.</returns>
        protected async Task<bool> DeleteMessageAsync(QueueMessage message)
        {
            var receivedMessageId = message.MessageId;
            var receivedMessagePopReceipt = message.PopReceipt;

            using var response = await QueueClient.DeleteMessageAsync(receivedMessageId, receivedMessagePopReceipt);

            return response.Status == 200; // might be 202 as might not execute
        }
    }
}
