﻿using Audacia.Azure.BlobStorage.Models;

namespace Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions
{
    public class BlobDataCannotBeNullException : Exception
    {
        private const string TemplateExceptionMessage = "Cannot add Blob: {0} which is of type {1} be null";

        public BlobDataCannotBeNullException(string blobName, BlobDataType blobDataType, IFormatProvider formatProvider) : base(
            string.Format(formatProvider, TemplateExceptionMessage, blobName, blobDataType.ToString()))
        {
        }
    }
}
