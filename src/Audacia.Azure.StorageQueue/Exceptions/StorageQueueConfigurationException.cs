namespace Audacia.Azure.StorageQueue.Exceptions
{
    /// <summary>
    /// Configuration exception for Azure Storage queue.
    /// </summary>
    public class StorageQueueConfigurationException : Exception
    {
        private const string OptionsNotConfiguredExceptionMessage =
            "Need to add a value of IOptions<QueueStorageOption> to the DI";

        private const string MissingConfigExceptionMessage =
            "Cannot connect to an Azure Queue storage with an null/empty {0}";

        private const string QueueClientNotConfiguredExceptionMessage =
            "Need to add QueueClient to the DI";

        /// <summary>
        /// Configuration exception for Azure Storage queue.
        /// </summary>
        public StorageQueueConfigurationException()
        {
        }

        /// <summary>
        /// Configuration exception for Azure Storage queue.
        /// </summary>
        /// <param name="exceptionMessage">Message for the Exception.</param>
        public StorageQueueConfigurationException(string exceptionMessage) : base(exceptionMessage)
        {
        }

        /// <summary>
        /// Configuration exception for Azure Storage queue.
        /// </summary>
        /// <param name="message">Message for the Exception.</param>
        /// <param name="innerException">Inner exception thrown.</param>
        public StorageQueueConfigurationException(string message, Exception innerException) : base(
            message,
            innerException)
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
        /// Exception for when Account Name has not been configured.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>Instance of <see cref="StorageQueueConfigurationException"/>.</returns>
        public static StorageQueueConfigurationException AccountNameNotConfigured(IFormatProvider formatProvider)
        {
            var exceptionMessage = string.Format(
                formatProvider,
                MissingConfigExceptionMessage,
                "account name");
            return new StorageQueueConfigurationException(exceptionMessage);
        }

        /// <summary>
        /// Exception for when Account Key has not been configured.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>Instance of <see cref="StorageQueueConfigurationException"/>.</returns>
        public static StorageQueueConfigurationException AccountKeyNotConfigured(IFormatProvider formatProvider)
        {
            var exceptionMessage = string.Format(
                formatProvider,
                MissingConfigExceptionMessage,
                "account key");
            return new StorageQueueConfigurationException(exceptionMessage);
        }

        /// <summary>
        /// Exception for when Storage Queue Client has not been configured.
        /// </summary>
        /// <returns>Instance of <see cref="StorageQueueConfigurationException"/>.</returns>
        public static StorageQueueConfigurationException QueueClientNotConfigured()
        {
            return new StorageQueueConfigurationException(QueueClientNotConfiguredExceptionMessage);
        }
    }
}
