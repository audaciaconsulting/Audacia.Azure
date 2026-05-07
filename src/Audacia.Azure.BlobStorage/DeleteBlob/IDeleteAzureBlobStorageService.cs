using System.Threading;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.DeleteBlob.Commands;

namespace Audacia.Azure.BlobStorage.DeleteBlob;

/// <summary>
/// Interface for removing blobs from an Azure Storage account.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1554:Method contains optional parameter in type hierarchy", Justification = "Allows common pattern of optional cancellation token parameter in interface methods.")]
public interface IDeleteAzureBlobStorageService
{
    /// <summary>
    /// Removes a blob with the <paramref name="command.BlobName"/> within <paramref name="command.ContainerName"/>.
    /// </summary>
    /// <param name="command">Command request containing all the information to remove a blob.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Whether the removing of the blob was successful.</returns>
    Task<bool> ExecuteAsync(DeleteAzureBlobStorageCommand command, CancellationToken cancellationToken = default);
}