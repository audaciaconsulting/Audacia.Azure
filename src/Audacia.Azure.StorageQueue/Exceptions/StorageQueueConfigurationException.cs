using System;

namespace Audacia.Azure.StorageQueue.Exceptions
{
    public class StorageQueueConfigurationException : Exception
    {
        private const string OptionsNotConfiguredExceptionMessage =
            "Need to add a value of IOptions<QueueStorageOption> to the DI";

        private const string MissingConfigExceptionMessage =
            "Cannot connect to an Azure Queue storage with an null/empty {0}";

        private const string QueueClientNotConfiguredExceptionMessage =
            "Need to add QueueClient to the DI";

        public StorageQueueConfigurationException(string exceptionMessage) : base(exceptionMessage)
        {
        }

        public static StorageQueueConfigurationException OptionsNotConfigured()
        {
            return new StorageQueueConfigurationException(OptionsNotConfiguredExceptionMessage);
        }

        public static StorageQueueConfigurationException AccountNameNotConfigured(IFormatProvider formatProvider)
        {
            return new StorageQueueConfigurationException(string.Format(formatProvider, MissingConfigExceptionMessage, "account name"));
        }

        public static StorageQueueConfigurationException AccountKeyNotConfigured(IFormatProvider formatProvider)
        {
            return new StorageQueueConfigurationException(string.Format(formatProvider, MissingConfigExceptionMessage, "account key"));
        }

        public static StorageQueueConfigurationException QueueClientNotConfigured()
        {
            return new StorageQueueConfigurationException(QueueClientNotConfiguredExceptionMessage);
        }
    }
}
