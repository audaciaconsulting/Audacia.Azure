﻿using System;

namespace Audacia.Azure.Common.ReturnOptions.ImageOption
{
    /// <summary>
    /// Return type of protected blob in URL format.
    /// </summary>
    public class ReturnProtectedUrlOption : IBlobReturnOption<string>
    {
        private string _blobName = default!;

        /// <summary>
        /// Gets the Name of the blob received from the storage account.
        /// </summary>
        public string BlobName => _blobName;

        /// <summary>
        /// Gets the returning value from blob storage containing the guid which it was saved within the storage account.
        /// </summary>
        public string Result { get; private set; } = default!;

        /// <summary>
        /// Assigns the generated Sas Uri to the result.
        /// </summary>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="bytes">Byte array representing the data of the blob.</param>
        /// <param name="blobClientUrl">URL of the blob client.</param>
        /// <returns>URl string.</returns>
        /// <exception cref="ArgumentNullException">Exception thrown if the <paramref name="blobClientUrl"/> is null.</exception>
        public string Parse(string blobName, byte[] bytes, Uri blobClientUrl)
        {
            if (blobClientUrl == null)
            {
                throw new ArgumentNullException(
                    nameof(blobClientUrl),
                    "Need to provide the blob service client url for return the url option");
            }

            _blobName = blobName;
            Result = blobClientUrl.OriginalString;

            return Result;
        }
    }
}
