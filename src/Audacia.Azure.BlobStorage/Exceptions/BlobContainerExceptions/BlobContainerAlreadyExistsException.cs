using System;

namespace Audacia.Azure.BlobStorage.Exceptions.BlobContainerExceptions
{
    /// <summary>
    /// Exception for when the Blob container already exists but expects to create the container.
    /// </summary>
    public class BlobContainerAlreadyExistsException : Exception
    {
        private const string TemplateExceptionMessage =
            "There is already a container on this storage account with the name: {0}";

        /// <summary>
        /// Exception for when the Blob container already exists but expects to create the container.
        /// </summary>
        public BlobContainerAlreadyExistsException()
        {
        }

        /// <summary>
        /// Exception for when the Blob container already exists but expects to create the container.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        public BlobContainerAlreadyExistsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Exception for when the Blob container already exists but expects to create the container.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">Inner exception thrown.</param>
        public BlobContainerAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Exception for when the Blob container already exists but expects to create the container.
        /// </summary>
        /// <param name="containerName">Name of the container that already exists.</param>
        /// <param name="formatProvider">The format provider.</param>
        public BlobContainerAlreadyExistsException(string containerName, IFormatProvider formatProvider) : base(
            string.Format(formatProvider, TemplateExceptionMessage, containerName))
        {
        }
    }
}
