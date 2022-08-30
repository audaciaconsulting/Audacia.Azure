namespace Audacia.Azure.BlobStorage.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class BlobNameAlreadyExistsException : Exception
    {
        private const string TemplateExceptionMessage =
            "Cannot add Blob: {0} within Container: {1} as there is an existing blob with that name therefore preventing overwriting of any data";

        /// <summary>
        /// Thrown when a blob is being added to a container which already has a blob with the same name.
        /// </summary>
        public BlobNameAlreadyExistsException()
        {
        }

        /// <summary>
        /// Thrown when a blob is being added to a container which already has a blob with the same name.
        /// </summary>
        /// <param name="message">Message for when the blob which already exists within a container.</param>
        public BlobNameAlreadyExistsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Thrown when a blob is being added to a container which already has a blob with the same name.
        /// </summary>
        /// <param name="message">Message for when the blob which already exists within a container.</param>
        /// <param name="innerException">Exception thrown detailing the full error.</param>
        public BlobNameAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Thrown when a blob is being added to a container which already has a blob with the same name.
        /// </summary>
        /// <param name="blobName">Name of blob which already exists within the <paramref name="containerName"/>.</param>
        /// <param name="containerName">Name of the Azure container where the blob tried to be added too.</param>
        /// <param name="formatProvider">Format provider to format the exception message.</param>
        public BlobNameAlreadyExistsException(string blobName, string containerName, IFormatProvider formatProvider) :
            base(
                string.Format(formatProvider, TemplateExceptionMessage, blobName, containerName))
        {
        }
    }
}
