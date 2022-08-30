namespace Audacia.Azure.BlobStorage.Exceptions
{
    public class BlobStorageConfigurationException : Exception
    {
        private const string OptionsNotConfiguredExceptionMessage =
            "Need to add a value of IOptions<BlobStorageConfig> to the DI";

        private const string MissingConfigExceptionMessage =
            "Cannot connect to an Azure Blob storage with an null/empty {0}";

        private const string BlobClientNotConfiguredExceptionMessage =
            "Need to add BlobServiceClient to the DI";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionMessage"></param>
        public BlobStorageConfigurationException(string exceptionMessage) : base(exceptionMessage)
        {
        }

        /// <summary>
        /// Exception option for when the <see cref="IOption"/> has not been configured.
        /// </summary>
        /// <returns>An instance of the exception when the options have not been configured.</returns>
        public static BlobStorageConfigurationException OptionsNotConfigured()
        {
            return new BlobStorageConfigurationException(OptionsNotConfiguredExceptionMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static BlobStorageConfigurationException AccountNameNotConfigured(IFormatProvider formatProvider)
        {
            return new BlobStorageConfigurationException(string.Format(formatProvider, MissingConfigExceptionMessage, "account name"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static BlobStorageConfigurationException AccountKeyNotConfigured(IFormatProvider formatProvider)
        {
            return new BlobStorageConfigurationException(string.Format(formatProvider, MissingConfigExceptionMessage, "account key"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static BlobStorageConfigurationException BlobClientNotConfigured()
        {
            return new BlobStorageConfigurationException(BlobClientNotConfiguredExceptionMessage);
        }
    }
}
