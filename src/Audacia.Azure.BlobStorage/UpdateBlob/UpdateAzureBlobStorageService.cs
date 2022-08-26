using System;
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
        /// <param name="blobServiceClient">Instance of <see cref="BlobServiceClient"/> for accessing blob in Azure.</param>
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
        /// <param name="blobStorageConfig"><see cref="BlobStorageOption"/> options for creating <see cref="BlobServiceClient"/>.</param>
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
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageFileCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var blobData = FileLocationBlobChecks(command.BlobName, command.FilePath);

            var container = GetContainer(command.ContainerName);

            if (container != null)
            {
                return await UpdateBlobInBlobStorageAsync(container, command, blobData).ConfigureAwait(false);
            }

            throw new ContainerDoesNotExistException(command.ContainerName, FormatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        /// <exception cref="BlobDataCannotBeNullException"></exception>
        /// <exception cref="BlobDataCannotBeEmptyException"></exception>
        /// <exception cref="BlobFileLocationNeedsToExistException"></exception>
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
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageBytesCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var blobData = BytesBlobChecks(command.BlobName, command.BlobData);

            var container = GetContainer(command.ContainerName);

            if (container != null)
            {
                return await UpdateBlobInBlobStorageAsync(container, command, blobData).ConfigureAwait(false);
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
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="ContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
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
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageStreamCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var blobData = StreamBlobDataCheck(command.BlobName, command.BlobData);

            var container = GetContainer(command.ContainerName);

            if (container != null)
            {
                var blobClient = container.GetBlobClient(command.BlobName);

                return await ReSyncBlobAsync(blobClient, command, blobData).ConfigureAwait(false);
            }

            throw new ContainerDoesNotExistException(command.ContainerName, FormatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobClient"></param>
        /// <param name="command"></param>
        /// <param name="blobData"></param>
        /// <returns></returns>
        /// <exception cref="BlobDoesNotExistException"></exception>
        private async Task<bool> ReSyncBlobAsync(
            BlobClient blobClient,
            UpdateAzureBlobStorageStreamCommand command,
            Stream blobData)
        {
            var blobExists = await blobClient.ExistsAsync().ConfigureAwait(false);

            if (blobExists.Value)
            {
                using (await blobClient.DeleteAsync().ConfigureAwait(false))
                {
                    try
                    {
                        await blobClient.UploadAsync(blobData).ConfigureAwait(false);
                        Logger.LogInformation(
                            $"Upload blob data for blob: {command.BlobName} to container: {command.ContainerName}");
                        return true;
                    }
                    catch (RequestFailedException requestFailedException)
                    {
                        Logger.LogError(requestFailedException, requestFailedException.Message);
                        return false;
                    }
                }
            }

            throw new BlobDoesNotExistException(command.BlobName, command.ContainerName, FormatProvider);
        }

        /// <summary>
        /// Checks the data which is going to be uploaded to blob storage.
        /// </summary>
        /// <param name="blobName">Name of the blob which is being updated.</param>
        /// <param name="streamBlobData">Stream of data for the updated blob.</param>
        /// <returns>The stream of blob data.</returns>
        /// <exception cref="BlobDataCannotBeNullException">
        /// Exception thrown when the blob data <see cref="Stream"/> is null.
        /// </exception>
        /// <exception cref="BlobDataCannotBeEmptyException">
        /// Exception thrown when the blob data <see cref="Stream"/> has a length of 0.
        /// </exception>
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

        /// <summary>
        /// Removes the exist blob from the storage container and re uploads the blob with the new data.
        /// </summary>
        /// <param name="container">The container where the exist blob lives.</param>
        /// <param name="command">
        /// The base command for the service containing information such as whether the container already exits.
        /// </param>
        /// <param name="blobData">Byte array containing the data of the blob.</param>
        /// <returns>A bool whether the uploading of the blob has been success.</returns>
        /// <exception cref="BlobDoesNotExistException">
        /// Exception for when the blob which is being updated does not exist in the given container.
        /// </exception>
        private async Task<bool> UpdateBlobInBlobStorageAsync(
            BlobContainerClient container,
            BaseUpdateBlobStorageCommand command,
            byte[] blobData)
        {
            var blobClient = container.GetBlobClient(command.BlobName);

            var blobExists = await blobClient.ExistsAsync().ConfigureAwait(false);

            if (blobExists.Value)
            {
                return await HandleReUploadingBlobAsync(blobClient, command, blobData).ConfigureAwait(false);
            }

            throw new BlobDoesNotExistException(command.BlobName, command.ContainerName, FormatProvider);
        }

        /// <summary>
        /// Removes the existing blob and re uploads the new value.
        /// </summary>
        /// <param name="blobClient">BlobClient where the current blob is.</param>
        /// <param name="command">Command containing blob name and container name.</param>
        /// <param name="blobData">Byte array for the data of the blob.</param>
        /// <returns>Whether the re upload was successful.</returns>
        private async Task<bool> HandleReUploadingBlobAsync(
            BlobClient blobClient,
            BaseUpdateBlobStorageCommand command,
            byte[] blobData)
        {
            await blobClient.DeleteAsync().ConfigureAwait(false);

            await using var ms = new MemoryStream(blobData, false).ConfigureAwait(false);
            try
            {
                await blobClient.UploadAsync(ms).ConfigureAwait(false);
                Logger.LogInformation(
                    $"Upload blob data for blob: {command.BlobName} to container: {command.ContainerName}");
                return true;
            }
            catch (RequestFailedException requestFailedException)
            {
                Logger.LogError(requestFailedException, requestFailedException.Message);
                return false;
            }
        }
    }
}
