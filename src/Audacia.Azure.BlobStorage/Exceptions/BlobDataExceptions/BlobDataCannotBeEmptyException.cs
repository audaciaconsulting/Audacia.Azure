using System;
using Audacia.Azure.BlobStorage.Models;

namespace Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions
{
    /// <summary>
    /// Exception for when blob data is empty.
    /// </summary>
    public class BlobDataCannotBeEmptyException : Exception
    {
        private const string TemplateExceptionMessage = "Cannot add Blob: {0} which is of type {1} be empty";

        /// <summary>
        /// Exception for when blob data is empty.
        /// </summary>
        public BlobDataCannotBeEmptyException()
        {
        }

        /// <summary>
        /// Exception for when blob data is empty.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public BlobDataCannotBeEmptyException(string message) : base(message)
        {
        }

        /// <summary>
        /// Exception for when blob data is empty.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner Exception thrown.</param>
        public BlobDataCannotBeEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Exception for when blob data is empty.
        /// </summary>
        /// <param name="blobName">Name of the blob which has empty data.</param>
        /// <param name="blobDataType">The type of blob type.</param>
        /// <param name="formatProvider">The format provider.</param>
        public BlobDataCannotBeEmptyException(
            string blobName,
            BlobDataType blobDataType,
            IFormatProvider formatProvider)
            : base(string.Format(formatProvider, TemplateExceptionMessage, blobName, blobDataType))
        {
        }
    }
}
