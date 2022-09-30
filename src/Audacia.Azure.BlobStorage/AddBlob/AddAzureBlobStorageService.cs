using Audacia.Azure.BlobStorage.AddBlob.Commands;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.UpdateBlob;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.AddBlob
{
    /// <summary>
    /// Add service for uploading blobs to an Azure Blob Storage account.
    /// </summary>
    public class AddAzureBlobStorageService : BaseAzureUpdateStorageService, IAddAzureBlobStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="BlobServiceClient"/> has being added to the DI.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobServiceClient">An instance of the <see cref="BlobServiceClient"/>.</param>
        public AddAzureBlobStorageService(
            ILogger<AddAzureBlobStorageService> logger,
            BlobServiceClient blobServiceClient) : base(
            logger, blobServiceClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="BlobStorageOption"/>.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobStorageConfig">
        /// Config option containing information on how to create <see cref="BlobServiceClient"/>.
        /// </param>
        public AddAzureBlobStorageService(
            ILogger<AddAzureBlobStorageService> logger,
            IOptions<BlobStorageOption> blobStorageConfig)
            : base(logger, blobStorageConfig)
        {
        }

        /// <summary>
        /// Adds a new blob to an Azure blob storage container.
        /// </summary>
        /// <param name="command">Command with information about the blob and the container where the blob is being added.</param>
        /// <returns>A boolean on whether the blob has been successfully added.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        public async Task<bool> ExecuteAsync(AddAzureBlobStorageBaseSixtyFourCommand command)
        {
            if (command is not null)
            {
                ContainerChecks(command.ContainerName, command.DoesContainerExist);

                var container = await GetOrCreateContainerAsync(command.ContainerName, command.DoesContainerExist)
                    .ConfigureAwait(false);
                var blobData = Convert.FromBase64String(command.BlobData);

                return await UploadBlobToBlobStorageAsync(container, command, blobData).ConfigureAwait(false);
            }

            throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you would like to upload a blob where the data is located
        /// on the local file server.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="BlobNameAlreadyExistsException">
        /// A blob with the same name as the one on the command exists within the specified container.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        public async Task<bool> ExecuteAsync(AddAzureBlobStorageFileCommand command)
        {
            if (command != null)
            {
                ContainerChecks(command.ContainerName, command.DoesContainerExist);

                var container = await GetOrCreateContainerAsync(command.ContainerName, command.DoesContainerExist)
                    .ConfigureAwait(false);
                var blobData = GetFileData(command.FilePath);

                return await UploadBlobToBlobStorageAsync(container, command, blobData).ConfigureAwait(false);
            }

            throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you have a byte array containing the data you want to add to
        /// the storage account.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="BlobNameAlreadyExistsException">
        /// A blob with the same name as the one on the command exists within the specified container.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        public async Task<bool> ExecuteAsync(AddAzureBlobStorageBytesCommand command)
        {
            if (command != null)
            {
                ContainerChecks(command.ContainerName, command.DoesContainerExist);

                var container = await GetOrCreateContainerAsync(command.ContainerName, command.DoesContainerExist)
                    .ConfigureAwait(false);

                return await UploadBlobToBlobStorageAsync(container, command, command.BlobData).ConfigureAwait(false);
            }

            throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you have a stream containing the data of the blob you want
        /// to upload to the storage account.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="BlobNameAlreadyExistsException">
        /// A blob with the same name as the one on the command exists within the specified container.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Command for service is null therefore cannot continue.
        /// </exception>
        public async Task<bool> ExecuteAsync(AddAzureBlobStorageStreamCommand command)
        {
            if (command != null)
            {
                ContainerChecks(command?.ContainerName, command.DoesContainerExist);

                var container = await GetOrCreateContainerAsync(command.ContainerName, command.DoesContainerExist)
                    .ConfigureAwait(false);
                var blobClient = container.GetBlobClient(command.BlobName);

                return await UploadStreamBlobAsync(blobClient, command).ConfigureAwait(false);
            }

            throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Uploads a blob to a container within an Azure Blob storage.
        /// </summary>
        /// <param name="blobClient">Blob client which the blob data will be uploaded too.</param>
        /// <param name="command">Command containing information such as the blob and container name.</param>
        /// <returns>A boolean based on if the blob has been successfully uploaded.</returns>
        /// <exception cref="BlobNameAlreadyExistsException">
        /// A blob with the same name as the one on the command exists within the specified container.
        /// </exception>
        private async Task<bool> UploadStreamBlobAsync(
            BlobClient blobClient,
            AddAzureBlobStorageStreamCommand command)
        {
            var blobExists = await blobClient.ExistsAsync().ConfigureAwait(false);

            if (!blobExists.Value)
            {
                try
                {
                    await blobClient.UploadAsync(command.BlobData).ConfigureAwait(false);
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

            throw new BlobNameAlreadyExistsException(command.BlobName, command.ContainerName, FormatProvider);
        }

        /// <summary>
        /// Adds a blob to Azure Blob Storage account.
        /// </summary>
        /// <param name="container">Container which the blob is going to be uploaded too.</param>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <param name="blobData">Byte array of the blob being uploaded.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        /// <exception cref="BlobNameAlreadyExistsException">
        /// A blob with the same name as the one on the command exists within the specified container.
        /// </exception>
        private async Task<bool> UploadBlobToBlobStorageAsync(
            BlobContainerClient container,
            BaseAddBlobStorageCommand command,
            IEnumerable<byte> blobData)
        {
            var blobClient = container.GetBlobClient(command.BlobName);

            var blobExists = await blobClient.ExistsAsync().ConfigureAwait(false);

            if (!blobExists.Value)
            {
                Logger.LogInformation(
                    $"Uploading blob: {command.BlobName} data to container: {command.ContainerName}");
                return await UploadFileBlobAsync(blobData, blobClient).ConfigureAwait(false);
            }

            throw new BlobNameAlreadyExistsException(command.BlobName, command.ContainerName, FormatProvider);
        }

        /// <summary>
        /// Uploads the blob data to the container in Azure.
        /// </summary>
        /// <param name="blobData">Byte array containing the data for the blob.</param>
        /// <param name="blobClient">Blob client where the blob data is being uploaded too.</param>
        /// <returns>A boolean on whether the blob has been successfully uploaded.</returns>
        private static async Task<bool> UploadFileBlobAsync(IEnumerable<byte> blobData, BlobClient blobClient)
        {
            using var ms = new MemoryStream(blobData.ToArray(), false);
            await blobClient.UploadAsync(ms).ConfigureAwait(false);

            return true;
        }
    }
}
