namespace Audacia.Azure.BlobStorage.Exceptions
{
    /// <summary>
    /// Exception thrown when blob does not exist in an Azure storage account.
    /// </summary>
    public class BlobDoesNotExistException : Exception
    {
        private const string TemplateExceptionMessage =
            "Unable to delete Blob: {0} within Container: {1} as it does not exist";

        /// <summary>
        /// Exception thrown when blob does not exist in an Azure storage account.
        /// </summary>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="formatProvider">The format provider.</param>
        public BlobDoesNotExistException(string blobName, string containerName, IFormatProvider formatProvider) : base(
            string.Format(formatProvider, TemplateExceptionMessage, blobName, containerName))
        {
        }

        public BlobDoesNotExistException()
        {
        }

        public BlobDoesNotExistException(string message) : base(message)
        {
        }

        public BlobDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
