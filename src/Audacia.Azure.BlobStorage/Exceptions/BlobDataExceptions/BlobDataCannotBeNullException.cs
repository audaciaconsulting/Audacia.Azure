using System;
using Audacia.Azure.BlobStorage.Models;

namespace Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions
{
    /// <summary>
    /// Exception for when blob data is null.
    /// </summary>
    public class BlobDataCannotBeNullException : Exception
    {
        private const string TemplateExceptionMessage = "Cannot add Blob: {0} which is of type {1} be null";

        /// <summary>
        /// Exception for when blob data is null.
        /// </summary>
        public BlobDataCannotBeNullException()
        {
        }

        /// <summary>
        /// Exception for when blob data is null.
        /// </summary>
        /// <param name="message">Message for the exception.</param>
        public BlobDataCannotBeNullException(string message) : base(message)
        {
        }

        /// <summary>
        /// Exception for when blob data is null.
        /// </summary>
        /// <param name="message">Message for the exception.</param>
        /// <param name="innerException">Inner Exception thrown.</param>
        public BlobDataCannotBeNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Exception for when blob data is null.
        /// </summary>
        /// <param name="blobName">Name of the blob which has null for data.</param>
        /// <param name="blobDataType">The type of blob type.</param>
        /// <param name="formatProvider">The format provider.</param>
        public BlobDataCannotBeNullException(string blobName, BlobDataType blobDataType, IFormatProvider formatProvider)
            : base(
                string.Format(formatProvider, TemplateExceptionMessage, blobName, blobDataType))
        {
        }
    }
}
