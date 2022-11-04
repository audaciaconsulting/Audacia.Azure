using Audacia.Azure.BlobStorage.Config;

namespace Audacia.Azure.BlobStorage.Exceptions
{
    /// <summary>
    /// Exception for when there is config missing in creating an Blob services.
    /// </summary>
    public class BlobStorageConfigurationException : Exception
    {
        private const string OptionsNotConfiguredExceptionMessage =
            "Need to add a value of IOptions<BlobStorageConfig> to the DI";

        private const string MissingConfigExceptionMessage =
            "Cannot connect to an Azure Blob storage with an null/empty {0}";

        private const string BlobClientNotConfiguredExceptionMessage =
            "Need to add BlobServiceClient to the DI";

        /// <summary>
        /// Exception for when there is config missing in creating an Blob services.
        /// </summary>
        public BlobStorageConfigurationException()
        {
        }

        /// <summary>
        /// Exception for when there is config missing in creating an Blob services.
        /// </summary>
        /// <param name="exceptionMessage">Message of the Exception.</param>
        public BlobStorageConfigurationException(string exceptionMessage) : base(exceptionMessage)
        {
        }

        /// <summary>
        /// Exception for when there is config missing in creating an Blob services.
        /// </summary>
        /// <param name="message">Message of the Exception.</param>
        /// <param name="innerException">Inner Exception thrown.</param>
        public BlobStorageConfigurationException(string message, Exception innerException) : base(
            message,
            innerException)
        {
        }

        /// <summary>
        /// Exception option for when the <see cref="BlobStorageOption"/> has not been configured.
        /// </summary>
        /// <returns>An instance of the exception when the options have not been configured.</returns>
        public static BlobStorageConfigurationException OptionsNotConfigured()
        {
            return new BlobStorageConfigurationException(OptionsNotConfiguredExceptionMessage);
        }

        /// <summary>
        /// Exception for when the account name has not been configured.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>Instance of <see cref="BlobStorageConfigurationException"/>.</returns>
        public static BlobStorageConfigurationException AccountNameNotConfigured(IFormatProvider formatProvider)
        {
            var exceptionMessage = string.Format(formatProvider, MissingConfigExceptionMessage, "account name");
            return new BlobStorageConfigurationException(exceptionMessage);
        }

        /// <summary>
        /// Exception for when the account key has not been configured.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>Instance of <see cref="BlobStorageConfigurationException"/>.</returns>
        public static BlobStorageConfigurationException AccountKeyNotConfigured(IFormatProvider formatProvider)
        {
            var exceptionMessage = string.Format(formatProvider, MissingConfigExceptionMessage, "account key");
            return new BlobStorageConfigurationException(exceptionMessage);
        }

        /// <summary>
        /// Exception for when the blob client has not been configured to the IoC.
        /// </summary>
        /// <returns>Instance of <see cref="BlobStorageConfigurationException"/>.</returns>
        public static BlobStorageConfigurationException BlobClientNotConfigured()
        {
            return new BlobStorageConfigurationException(BlobClientNotConfiguredExceptionMessage);
        }
    }
}
