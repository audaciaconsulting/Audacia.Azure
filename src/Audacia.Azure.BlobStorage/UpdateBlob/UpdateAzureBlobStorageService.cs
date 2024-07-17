using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Exceptions.BlobContainerExceptions;
using Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions;
using Audacia.Azure.BlobStorage.Extensions;
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
            IOptions<BlobStorageOption> blobStorageConfig) : base(logger, blobStorageConfig)
        {
        }

        /// <summary>
        /// Update an existing blob with the file data from the file stored at <paramref name="command.FilePath"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        /// <exception cref="BlobContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        public async Task<bool> ExecuteAsync(
            UpdateBlobStorageFileCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command != null)
            {
                await ContainerChecksAsync(command.ContainerName, command.DoesContainerExist, cancellationToken)
                    .ConfigureAwait(false);

                var blobData = UpdateAzureBlobStorageService.FileLocationBlobChecks(command.BlobName, command.FilePath);

                var container = GetContainer(command.ContainerName);

                if (container != null)
                {
                    return await UpdateBlobInBlobStorageAsync(container, command, blobData, cancellationToken)
                        .ConfigureAwait(false);
                }

                throw new BlobContainerDoesNotExistException(command.ContainerName, FormatProvider);
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
        private static IEnumerable<byte> FileLocationBlobChecks(
            string blobName,
            string fileLocation)
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
                throw new BlobFileLocationNeedsToExistException(
                    blobName,
                    BlobDataType.FileLocation,
                    fileLocation,
                    FormatProvider);
            }

            var blobData = GetFileData(fileLocation);

            return blobData;
        }

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="BlobContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        public async Task<bool> ExecuteAsync(
            UpdateBlobStorageBytesCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command != null)
            {
                await ContainerChecksAsync(command.ContainerName, command.DoesContainerExist, cancellationToken)
                    .ConfigureAwait(false);

                var blobData = UpdateAzureBlobStorageService.BytesBlobChecks(command.BlobName, command.BlobData);

                var container = GetContainer(command.ContainerName);

                if (container != null)
                {
                    return await UpdateBlobInBlobStorageAsync(container, command, blobData, cancellationToken)
                        .ConfigureAwait(false);
                }

                throw new BlobContainerDoesNotExistException(command.ContainerName, FormatProvider);
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
        private static IEnumerable<byte> BytesBlobChecks(
            string blobName,
            IEnumerable<byte> bytesBlobData)
        {
            if (bytesBlobData == null)
            {
                throw new BlobDataCannotBeNullException(blobName, BlobDataType.ByteArray, FormatProvider);
            }

            if (!bytesBlobData.Any())
            {
                throw new BlobDataCannotBeEmptyException(blobName, BlobDataType.ByteArray, FormatProvider);
            }

            return bytesBlobData;
        }

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="BlobContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        public async Task<bool> ExecuteAsync(
            UpdateBlobStorageBaseSixtyFourCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command != null)
            {
                var containerExists = await ContainerChecksAsync(
                    command.ContainerName,
                    command.DoesContainerExist,
                    cancellationToken).ConfigureAwait(false);

                var blobData = command.BlobData.BaseSixtyFourBlobChecks(command.BlobName, FormatProvider);

                var container = GetContainer(command.ContainerName);

                if (container != null)
                {
                    return await UpdateBlobInBlobStorageAsync(
                        container,
                        command,
                        (IEnumerable<byte>)blobData,
                        cancellationToken).ConfigureAwait(false);
                }

                throw new BlobContainerDoesNotExistException(command.ContainerName, FormatProvider);
            }

            throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="BlobContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        public async Task<bool> ExecuteAsync(
            UpdateBlobStorageStreamCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command != null)
            {
                await ContainerChecksAsync(command.ContainerName, command.DoesContainerExist, cancellationToken)
                    .ConfigureAwait(false);

                var blobData = UpdateAzureBlobStorageService.StreamBlobDataCheck(command.BlobName, command.BlobData);

                var container = GetContainer(command.ContainerName);

                if (container != null)
                {
                    var blobClient = container.GetBlobClient(command.BlobName);

                    return await ReSyncBlobAsync(blobClient, command, blobData, cancellationToken)
                        .ConfigureAwait(false);
                }

                throw new BlobContainerDoesNotExistException(command.ContainerName, FormatProvider);
            }

            throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Remove the existing blob and re uploads the blob with the new blob data.
        /// </summary>
        /// <param name="blobClient">The <see cref="BlobClient"/> to remove and upload a blob.</param>
        /// <param name="command">Command containing information such as the blob and container name.</param>
        /// <param name="blobData">Stream of the blob data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Boolean whether the removing and uploading of the blob has been successful.</returns>
        /// <exception cref="BlobDoesNotExistException">
        /// The blob been updated does not exist with the specified container.
        /// </exception>
        private async Task<bool> ReSyncBlobAsync(
            BlobClient blobClient,
            UpdateBlobStorageStreamCommand command,
            Stream blobData,
            CancellationToken cancellationToken)
        {
            var blobExists = await blobClient.ExistsAsync(cancellationToken).ConfigureAwait(false);

            if (blobExists.Value)
            {
                using (await blobClient.DeleteAsync(cancellationToken: cancellationToken).ConfigureAwait(false))
                {
                    try
                    {
                        await blobClient.UploadAsync(blobData, cancellationToken).ConfigureAwait(false);
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
        private static Stream StreamBlobDataCheck(
            string blobName,
            Stream streamBlobData)
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
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A bool whether the uploading of the blob has been success.</returns>
        /// <exception cref="BlobDoesNotExistException">
        /// Exception for when the blob which is being updated does not exist in the given container.
        /// </exception>
        private async Task<bool> UpdateBlobInBlobStorageAsync(
            BlobContainerClient container,
            BaseUpdateBlobStorageCommand command,
            IEnumerable<byte> blobData,
            CancellationToken cancellationToken)
        {
            var blobClient = container.GetBlobClient(command.BlobName);

            var blobExists = await blobClient.ExistsAsync(cancellationToken).ConfigureAwait(false);

            if (blobExists.Value)
            {
                return await HandleReUploadingBlobAsync(blobClient, command, blobData, cancellationToken)
                    .ConfigureAwait(false);
            }

            throw new BlobDoesNotExistException(command.BlobName, command.ContainerName, FormatProvider);
        }

        /// <summary>
        /// Removes the existing blob and re uploads the new value.
        /// </summary>
        /// <param name="blobClient">BlobClient where the current blob is.</param>
        /// <param name="command">Command containing blob name and container name.</param>
        /// <param name="blobData">Byte array for the data of the blob.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Whether the re upload was successful.</returns>
        [SuppressMessage(
            "Reliability",
            "CA2007:Consider calling ConfigureAwait on the awaited task",
            Justification = "Cannot use configure await on Memory stream.")]
        private async Task<bool> HandleReUploadingBlobAsync(
            BlobClient blobClient,
            BaseUpdateBlobStorageCommand command,
            IEnumerable<byte> blobData,
            CancellationToken cancellationToken)
        {
            using (await blobClient.DeleteAsync(cancellationToken: cancellationToken).ConfigureAwait(false))
            {
                var blobDataArray = blobData.ToArray();
                await using var ms = new MemoryStream(blobDataArray, false);
                try
                {
                    await blobClient.UploadAsync(ms, cancellationToken).ConfigureAwait(false);
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