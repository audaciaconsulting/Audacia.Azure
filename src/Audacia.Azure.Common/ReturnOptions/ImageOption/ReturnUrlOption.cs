﻿using System;

namespace Audacia.Azure.Common.ReturnOptions.ImageOption
{
    public class ReturnUrlOption : IBlobReturnOption<string>
    {
        private string _blobName;

        /// <summary>
        /// Gets the Name of the blob received from the storage account.
        /// </summary>
        public string BlobName => _blobName;

        /// <summary>
        /// Gets the returning value from blob storage containing the guid which it was saved within the storage account.
        /// </summary>
        public string Result { get; private set; }

        public string Parse(string blobName, byte[] bytes, Uri blobClientUrl)
        {
            if (blobClientUrl == null)
            {
                throw new ArgumentNullException(
                    nameof(blobClientUrl),
                    "Need to provide the blob service client url for return the url option");
            }

            _blobName = blobName;
            Result = $"{blobClientUrl}/{blobName}";

            return Result;
        }
    }
}
