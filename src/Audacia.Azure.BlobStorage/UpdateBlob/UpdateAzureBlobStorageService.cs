﻿using System;
using System.IO;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions;
using Audacia.Azure.BlobStorage.Models;
using Audacia.Azure.BlobStorage.UpdateBlob.Commands;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.UpdateBlob
{
    /// <summary>
    /// Update service for returning blob from an Azure Blob Storage account.
    /// </summary>
    public class UpdateAzureBlobStorageService : BaseAzureUpdateStorageService, IUpdateAzureBlobStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="BlobServiceClient"/> has being added to the DI.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobServiceClient"></param>
        public UpdateAzureBlobStorageService(
            ILogger<UpdateAzureBlobStorageService> logger,
            BlobServiceClient blobServiceClient)
            : base(logger, blobServiceClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="BlobStorageOption"/>.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobStorageConfig"></param>
        public UpdateAzureBlobStorageService(
            ILogger<UpdateAzureBlobStorageService> logger,
            IOptions<BlobStorageOption> blobStorageConfig) : base(
            logger, blobStorageConfig)
        {
        }

        /// <summary>
        /// Update an existing blob with the file data from the file stored at <paramref name="command.FilePath"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageFileCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var blobData = FileLocationBlobChecks(command.BlobName, command.FilePath);

            var container = GetContainer(command.ContainerName);

            if (container != null)
            {
                return await UpdateBlobInBlobStorageAsync(container, command, blobData);
            }

            throw new ContainerDoesNotExistException(command.ContainerName, FormatProvider);
        }

        private byte[] FileLocationBlobChecks(string blobName, string fileLocation)
        {
            if (fileLocation == null)
            {
                throw new BlobDataCannotBeNullException(blobName, BlobDataType.FileLocation, FormatProvider);
            }

            if (fileLocation.Length == 0)
            {
                throw new BlobDataCannotBeEmptyException(blobName, BlobDataType.FileLocation, FormatProvider);
            }

            if (!File.Exists(fileLocation))
            {
                throw new BlobFileLocationNeedsToExistException(blobName, BlobDataType.FileLocation, fileLocation,
                    FormatProvider);
            }

            var blobData = GetFileData(fileLocation);

            return blobData;
        }

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageBytesCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var blobData = BytesBlobChecks(command.BlobName, command.BlobData);

            var container = GetContainer(command.ContainerName);

            if (container != null)
            {
                return await UpdateBlobInBlobStorageAsync(container, command, blobData);
            }

            throw new ContainerDoesNotExistException(command.ContainerName, FormatProvider);
        }

        private byte[] BytesBlobChecks(string blobName, byte[] bytesBlobData)
        {
            if (bytesBlobData == null)
            {
                throw new BlobDataCannotBeNullException(blobName, BlobDataType.ByteArray, FormatProvider);
            }

            if (bytesBlobData.Length == 0)
            {
                throw new BlobDataCannotBeEmptyException(blobName, BlobDataType.ByteArray, FormatProvider);
            }

            return bytesBlobData;
        }

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageBaseSixtyFourCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var blobData = BaseSixtyFourBlobChecks(command.BlobName, command.BlobData);

            var container = GetContainer(command.ContainerName);

            if (container != null)
            {
                return await UpdateBlobInBlobStorageAsync(container, command, blobData);
            }

            throw new ContainerDoesNotExistException(command.ContainerName, FormatProvider);
        }

        private byte[] BaseSixtyFourBlobChecks(string blobName, string baseSixtyFourBlobData)
        {
            if (baseSixtyFourBlobData == null)
            {
                throw new BlobDataCannotBeNullException(blobName, BlobDataType.BaseSixtyFour, FormatProvider);
            }

            if (baseSixtyFourBlobData.Length == 0)
            {
                throw new BlobDataCannotBeEmptyException(blobName, BlobDataType.BaseSixtyFour, FormatProvider);
            }

            var buffer = new Span<byte>(new byte[baseSixtyFourBlobData.Length]);
            var isValidBaseSixtyFour = Convert.TryFromBase64String(baseSixtyFourBlobData, buffer, out _);

            if (isValidBaseSixtyFour)
            {
                throw new BlobDataCannotBeInvalidBaseSixtyFourException(blobName, baseSixtyFourBlobData,
                    FormatProvider);
            }

            return buffer.ToArray();
        }

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageStreamCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var blobData = StreamBlobDataCheck(command.BlobName, command.BlobData);

            var container = GetContainer(command.ContainerName);

            if (container != null)
            {
                var blobClient = container.GetBlobClient(command.BlobName);

                return await RemoveBlobAsync(blobClient, command, blobData);
            }

            throw new ContainerDoesNotExistException(command.ContainerName, FormatProvider);
        }

        private async Task<bool> RemoveBlobAsync(
            BlobClient blobClient,
            UpdateAzureBlobStorageStreamCommand command,
            Stream blobData)
        {
            var blobExists = await blobClient.ExistsAsync();

            if (blobExists.Value)
            {
                await blobClient.DeleteAsync();

                try
                {
                    await blobClient.UploadAsync(blobData);
                    return true;
                }
                catch (RequestFailedException)
                {
                    return false;
                }
            }

            throw new BlobDoesNotExistException(command.BlobName, command.ContainerName, FormatProvider);
        }

        private Stream StreamBlobDataCheck(string blobName, Stream streamBlobData)
        {
            if (streamBlobData == null)
            {
                throw new BlobDataCannotBeNullException(blobName, BlobDataType.Stream, FormatProvider);
            }

            if (streamBlobData.Length <= 0)
            {
                throw new BlobDataCannotBeEmptyException(blobName, BlobDataType.Stream, FormatProvider);
            }

            return streamBlobData;
        }

        private async Task<bool> UpdateBlobInBlobStorageAsync(
            BlobContainerClient container,
            BaseUpdateBlobStorageCommand command,
            byte[] blobData)
        {
            var blobClient = container.GetBlobClient(command.BlobName);

            var blobExists = await blobClient.ExistsAsync();

            if (blobExists.Value)
            {
                await blobClient.DeleteAsync();

                await using var ms = new MemoryStream(blobData, false);
                try
                {
                    await blobClient.UploadAsync(ms);
                    return true;
                }
                catch (RequestFailedException)
                {
                    return false;
                }
            }

            throw new BlobDoesNotExistException(command.BlobName, command.ContainerName, FormatProvider);
        }
    }
}
