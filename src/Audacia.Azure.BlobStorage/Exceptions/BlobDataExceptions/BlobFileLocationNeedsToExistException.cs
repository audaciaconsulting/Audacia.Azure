using Audacia.Azure.BlobStorage.Models;

namespace Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions
{
    /// <summary>
    /// Exception thrown when the file location does not exist.
    /// </summary>
    public class BlobFileLocationNeedsToExistException : Exception
    {
        private const string TemplateExceptionMessage =
            "Cannot add Blob: {0} which is of type {1} be null as doesnt exist on file system {2}";

        /// <summary>
        /// Exception thrown when the file location does not exist.
        /// </summary>
        public BlobFileLocationNeedsToExistException()
        {
        }

        /// <summary>
        /// Exception thrown when the file location does not exist.
        /// </summary>
        /// <param name="message">Message for the exception.</param>
        public BlobFileLocationNeedsToExistException(string message) : base(message)
        {
        }

        /// <summary>
        /// Exception thrown when the file location does not exist.
        /// </summary>
        /// <param name="message">Message for the exception.</param>
        /// <param name="innerException">Inner Exception thrown.</param>
        public BlobFileLocationNeedsToExistException(string message, Exception innerException) : base(
            message,
            innerException)
        {
        }

        /// <summary>
        /// Exception thrown when the file location does not exist.
        /// </summary>
        /// <param name="blobName">Name of the blob which has empty data.</param>
        /// <param name="blobDataType">The type of blob type.</param>
        /// <param name="fileLocation">The location of the file.</param>
        /// <param name="formatProvider">The format provider.</param>
        public BlobFileLocationNeedsToExistException(string blobName, BlobDataType blobDataType, string fileLocation,
            IFormatProvider formatProvider)
            : base(string.Format(formatProvider, TemplateExceptionMessage, blobName, blobDataType.ToString(),
                fileLocation))
        {
        }
    }
}
