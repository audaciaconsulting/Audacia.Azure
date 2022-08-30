﻿namespace Audacia.Azure.BlobStorage.Exceptions
{
    public class BlobDoesNotExistException : Exception
    {
        private const string TemplateExceptionMessage =
            "Unable to delete Blob: {0} within Container: {1} as it does not exist";

        public BlobDoesNotExistException(string blobName, string containerName, IFormatProvider formatProvider) : base(
            string.Format(formatProvider, TemplateExceptionMessage, blobName, containerName))
        {
        }
    }
}
