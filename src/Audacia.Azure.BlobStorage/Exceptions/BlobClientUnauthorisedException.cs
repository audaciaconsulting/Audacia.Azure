using System;

namespace Audacia.Azure.BlobStorage.Exceptions.BlobContainerExceptions
{
    /// <summary>
    /// Exception for when the blob container is not authorized to generate a SAS token.
    /// </summary>
    public class BlobClientUnauthorisedException : Exception
    {
        private const string TemplateExceptionMessage =
            "Cannot generate a SAS token for container with a name of {0}";

        /// <summary>
        /// Exception for when the blob container is not authorized to generate a SAS token.
        /// </summary>
        public BlobClientUnauthorisedException()
        {
        }

        /// <summary>
        /// Exception for when the blob container is not authorized to generate a SAS token.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        public BlobClientUnauthorisedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Exception for when the blob container is not authorized to generate a SAS token.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">The format provider.</param>
        public BlobClientUnauthorisedException(string message, Exception innerException) : base(
            message,
            innerException)
        {
        }

        /// <summary>
        /// Exception for when container is unable to generate a SAS token.
        /// </summary>
        /// <param name="containerName">Name of the container which cannot generate a SAS token.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>Instance of <see cref="BlobClientUnauthorisedException"/>.</returns>
        public static BlobClientUnauthorisedException UnableToGenerateSasToken(
            string containerName,
            IFormatProvider formatProvider)
        {
            var exceptionMessage = string.Format(
                formatProvider,
                TemplateExceptionMessage,
                containerName);
            return new BlobClientUnauthorisedException(exceptionMessage);
        }
    }
}
