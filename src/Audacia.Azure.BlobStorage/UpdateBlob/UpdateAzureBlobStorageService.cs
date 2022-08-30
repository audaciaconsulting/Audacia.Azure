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
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        /// <exception cref="ContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageFileCommand command)
        {
            if (command != null)
            {
                ContainerChecks(command.ContainerName, command.DoesContainerExist);

                var blobData = FileLocationBlobChecks(command.BlobName, command.FilePath);

                var container = GetContainer(command.ContainerName);

                if (container is not null)
                {
                    return await UpdateBlobInBlobStorageAsync(container, command, blobData).ConfigureAwait(false);
                }

                throw new ContainerDoesNotExistException(command.ContainerName, FormatProvider);
            }

            throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Checks if the file location for getting the blob data exists.
        /// </summary>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="fileLocation">Location to find the data for the blob.</param>
        /// <returns>Byte array of the data at the specified file location.</returns>
        /// <exception cref="BlobDataCannotBeNullException"><paramref name="fileLocation"/> is null.</exception>
        /// <exception cref="BlobDataCannotBeEmptyException"><paramref name="fileLocation"/> is an empty string.</exception>
        /// <exception cref="BlobFileLocationNeedsToExistException">No file exists at <paramref name="fileLocation"/>.</exception>
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
        /// <exception cref="ContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageBytesCommand command)
        {
            if (command != null)
            {
                ContainerChecks(command.ContainerName, command.DoesContainerExist);

                var blobData = BytesBlobChecks(command.BlobName, command.BlobData);

                var container = GetContainer(command.ContainerName);

                if (container is not null)
                {
                    return await UpdateBlobInBlobStorageAsync(container, command, blobData).ConfigureAwait(false);
                }

                throw new ContainerDoesNotExistException(command.ContainerName, FormatProvider);
            }

            throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Checks the byte array of the blob data to see if it's valid.
        /// </summary>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="bytesBlobData">Byte array of the data of the blob.</param>
        /// <returns>Byte array with the data for the blob.</returns>
        /// <exception cref="BlobDataCannotBeNullException"><paramref name="bytesBlobData"/> is null.</exception>
        /// <exception cref="BlobDataCannotBeEmptyException"><paramref name="bytesBlobData"/> has an length of 0.</exception>
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
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageBaseSixtyFourCommand command)
        {
            if (command != null)
            {
                ContainerChecks(command.ContainerName, command.DoesContainerExist);

                var blobData = BaseSixtyFourBlobChecks(command.BlobName, command.BlobData);

                var container = GetContainer(command.ContainerName);

                if (container is not null)
                {
                    return await UpdateBlobInBlobStorageAsync(container, command, blobData).ConfigureAwait(false);
                }

                throw new ContainerDoesNotExistException(command.ContainerName, FormatProvider);
            }

            throw new ArgumentNullException(nameof(command));
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
        /// <exception cref="ContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageStreamCommand command)
        {
            if (command != null)
            {
                ContainerChecks(command.ContainerName, command.DoesContainerExist);

                var blobData = StreamBlobDataCheck(command.BlobName, command.BlobData);

                var container = GetContainer(command.ContainerName);

                if (container is not null)
                {
                    var blobClient = container.GetBlobClient(command.BlobName);

                    return await ReSyncBlobAsync(blobClient, command, blobData).ConfigureAwait(false);
                }

                throw new ContainerDoesNotExistException(command.ContainerName, FormatProvider);
            }

            throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Remove the existing blob and re uploads the blob with the new blob data.
        /// </summary>
        /// <param name="blobClient">The <see cref="BlobClient"/> to remove and upload a blob.</param>
        /// <param name="command">Command containing information such as the blob and container name.</param>
        /// <param name="blobData">Stream of the blob data.</param>
        /// <returns>Boolean whether the removing and uploading of the blob has been successful.</returns>
        /// <exception cref="BlobDoesNotExistException">
        /// The blob been updated does not exist with the specified container.
        /// </exception>
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
            using (await blobClient.DeleteAsync().ConfigureAwait(false))
            {
                using var ms = new MemoryStream(blobData, false);
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
}
