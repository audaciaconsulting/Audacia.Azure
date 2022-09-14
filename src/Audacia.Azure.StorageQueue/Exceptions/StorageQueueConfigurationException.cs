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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionMessage">Message for the Exception.</param>
        public StorageQueueConfigurationException(string exceptionMessage) : base(exceptionMessage)
        {
        }
        
        /// <summary>
        /// Exception option for when the <see cref="IOption"/> has not been configured.
        /// </summary>
        /// <returns>An instance of the exception when the options have not been configured.</returns>
        public static StorageQueueConfigurationException OptionsNotConfigured()
        {
            return new StorageQueueConfigurationException(OptionsNotConfiguredExceptionMessage);
        }

        /// <summary>
        /// asd
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static StorageQueueConfigurationException AccountNameNotConfigured(IFormatProvider formatProvider)
        {
            return new StorageQueueConfigurationException(string.Format(formatProvider, MissingConfigExceptionMessage, "account name"));
        }
        
/// <summary>
/// 
/// </summary>
/// <param name="formatProvider"></param>
/// <returns></returns>
        public static StorageQueueConfigurationException AccountKeyNotConfigured(IFormatProvider formatProvider)
        {
            return new StorageQueueConfigurationException(string.Format(formatProvider, MissingConfigExceptionMessage, "account key"));
        }

/// <summary>
/// /
/// </summary>
/// <returns></returns>
        public static StorageQueueConfigurationException QueueClientNotConfigured()
        {
            return new StorageQueueConfigurationException(QueueClientNotConfiguredExceptionMessage);
        }
    }
}
