namespace Audacia.Azure.BlobStorage.Exceptions.BlobContainerExceptions
{
    /// <summary>
    /// Exception for when the blob container name is invalid.
    /// </summary>
    public class BlobContainerNameInvalidException : Exception
    {
        private const string EmptyTemplateExceptionMessage =
            "Cannot {0} a new container {1} with a name that is null / empty";

        private const string TemplateExceptionMessage =
            "Cannot {0} container with a name of {1}";

        /// <summary>
        /// Exception for when the blob container name is invalid.
        /// </summary>
        public BlobContainerNameInvalidException()
        {
        }

        /// <summary>
        /// Exception for when the blob container name is invalid.
        /// </summary>
        /// <param name="exceptionMessage">Message of the exception.</param>
        private BlobContainerNameInvalidException(string exceptionMessage) : base(exceptionMessage)
        {
        }

        /// <summary>
        /// Exception for when the blob container name is invalid.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">The format provider.</param>
        public BlobContainerNameInvalidException(string message, Exception innerException) : base(
            message,
            innerException)
        {
        }

        /// <summary>
        /// Exception for when the blob container name is invalid that the container cannot be found in Azure.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="containerName">The name of Blob container which couldn't be found.</param>
        /// <returns>Instance of <see cref="BlobContainerNameInvalidException"/>.</returns>
        public static BlobContainerNameInvalidException UnableToFindWithContainerName(
            IFormatProvider formatProvider,
            string containerName)
        {
            var exceptionMessage = string.Format(
                formatProvider,
                TemplateExceptionMessage,
                "find",
                containerName);
            return new BlobContainerNameInvalidException(exceptionMessage);
        }

        /// <summary>
        /// Exception for when the blob container name is empty or null.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>Instance of <see cref="BlobContainerNameInvalidException"/>.</returns>
        public static BlobContainerNameInvalidException UnableToFindWithEmptyContainerName(
            IFormatProvider formatProvider)
        {
            var exceptionMessage = string.Format(
                formatProvider,
                EmptyTemplateExceptionMessage,
                "find",
                string.Empty);
            return new BlobContainerNameInvalidException(exceptionMessage);
        }

        /// <summary>
        /// Exception for when the blob container name is invalid.
        /// </summary>
        /// <param name="containerName">Name of the blob container which does not exist.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>Instance of <see cref="BlobContainerNameInvalidException"/>.</returns>
        public static BlobContainerNameInvalidException UnableToCreateWithContainerName(
            string containerName,
            IFormatProvider formatProvider)
        {
            var exceptionMessage = string.Format(
                formatProvider,
                EmptyTemplateExceptionMessage,
                "create",
                containerName);
            return new BlobContainerNameInvalidException(exceptionMessage);
        }
    }
}
