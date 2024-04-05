﻿using System.IO;

namespace Audacia.Azure.BlobStorage.AddBlob.Commands
{
    /// <summary>
    /// Command used for adding a container and a blob which has it's data in an stream.
    /// </summary>
    public class AddBlobWithContainerStreamCommand : BaseAddBlobStorageCommand
    {
        /// <summary>
        /// Gets stream of the blob which you want to upload to storage.
        /// </summary>
        public Stream BlobData { get; }

        /// <summary>
        /// Command used for adding a container and a blob which has it's data in an stream.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="blobData">Data of the blob.</param>
        public AddBlobWithContainerStreamCommand(
            string containerName, 
            string blobName,
            Stream blobData) : base(containerName, blobName, false)
        {
            BlobData = blobData;
        }
    }
}
