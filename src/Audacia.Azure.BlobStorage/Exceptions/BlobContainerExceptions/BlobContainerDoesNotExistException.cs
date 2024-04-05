using System;

namespace Audacia.Azure.BlobStorage.Exceptions.BlobContainerExceptions
{
    /// <summary>
    /// Exception for when the blob container does not exist.
    /// </summary>
    public class BlobContainerDoesNotExistException : Exception
    {
        private const string TemplateExceptionMessage =
            "Container: {0} does not exist therefore unable to find the blob within the specified container";

        /// <summary>
        /// Exception for when the blob container does not exist.
        /// </summary>
        public BlobContainerDoesNotExistException()
        {
        }

        /// <summary>
        /// Exception for when the blob container does not exist.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        public BlobContainerDoesNotExistException(string message) : base(message)
        {
        }

        /// <summary>
        /// Exception for when the blob container does not exist.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">Inner exception thrown.</param>
        public BlobContainerDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Exception for when the blob container does not exist.
        /// </summary>
        /// <param name="containerName">Name of the blob container which does not exist.</param>
        /// <param name="formatProvider">The format provider.</param>
        public BlobContainerDoesNotExistException(string containerName, IFormatProvider formatProvider) : base(
            string.Format(formatProvider, TemplateExceptionMessage, containerName))
        {
        }
    }
}
