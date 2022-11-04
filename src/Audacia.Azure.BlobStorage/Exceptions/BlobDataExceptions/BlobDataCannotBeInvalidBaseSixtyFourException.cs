namespace Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions
{
    /// <summary>
    /// Exception for when blob data is invalid base 64.
    /// </summary>
    public class BlobDataCannotBeInvalidBaseSixtyFourException : Exception
    {
        private const string TemplateExceptionMessage =
            "Cannot add Blob: {0} because the data value is invalid Base 64: {1}";

        /// <summary>
        /// Exception for when blob data is invalid base 64.
        /// </summary>
        public BlobDataCannotBeInvalidBaseSixtyFourException()
        {
        }

        /// <summary>
        /// Exception for when blob data is invalid base 64.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        public BlobDataCannotBeInvalidBaseSixtyFourException(string message) : base(message)
        {
        }

        /// <summary>
        /// Exception for when blob data is invalid base 64.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">Inner Exception thrown.</param>
        public BlobDataCannotBeInvalidBaseSixtyFourException(string message, Exception innerException) : base(
            message,
            innerException)
        {
        }

        /// <summary>
        /// Exception for when blob data is invalid base 64.
        /// </summary>
        /// <param name="blobName">Name of the blob which has empty data.</param>
        /// <param name="blobData">Invalid base 64 string.</param>
        /// <param name="formatProvider">The format provider.</param>
        public BlobDataCannotBeInvalidBaseSixtyFourException(
            string blobName, 
            string blobData,
            IFormatProvider formatProvider)
            : base(string.Format(formatProvider, TemplateExceptionMessage, blobName, blobData))
        {
        }
    }
}
