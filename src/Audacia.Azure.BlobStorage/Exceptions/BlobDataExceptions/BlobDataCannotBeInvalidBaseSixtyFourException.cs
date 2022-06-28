using System;
using System.Globalization;

namespace Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions
{
    public class BlobDataCannotBeInvalidBaseSixtyFourException : Exception
    {
        private const string TemplateExceptionMessage =
            "Cannot add Blob: {0} because the data value is invalid Base 64: {1}";

        public BlobDataCannotBeInvalidBaseSixtyFourException(string blobName, string blobData, IFormatProvider formatProvider)
            : base(string.Format(formatProvider, TemplateExceptionMessage, blobName, blobData))
        {
        }
    }
}
