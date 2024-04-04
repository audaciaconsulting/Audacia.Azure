using Audacia.Azure.Common.ReturnOptions;
using Audacia.Azure.Common.ReturnOptions.ImageOption;

namespace Audacia.Azure.BlobStorage.GetBlob;

/// <summary>
/// This should be added to the DI, along with registering it against the concrete type <see cref="GetAzureProtectedBlobStorageService"/>.
/// </summary>
public interface IGetAzureProtectedBlobStorageService
{
    /// <summary>
    /// Returns a blob with the name <paramref name="blobName"/> within the <paramref name="containerName"/>.
    /// </summary>
    /// <param name="containerName">The name of the container where the blob you want to return is stored in.</param>
    /// <param name="blobName">The name of the blob you are wanting to return.</param>
    /// <param name="policyName">The name of the security policy for the container.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
    /// <returns>A <see cref="IBlobReturnOption{T}"/> which has been configured by the generic arguments.</returns>
    Task<string> GetAsync<TResponse>(string containerName, string blobName, string? policyName, CancellationToken cancellationToken)
        where TResponse : ReturnProtectedUrlOption, new();

    /// <summary>
    /// Returns a collection of blob based on the collection of blob names <paramref name="blobNames"/> within the
    /// container within an Azure storage account.
    /// </summary>
    /// <param name="containerName">The name of the container where the blob you want to return is stored in.</param>
    /// <param name="blobNames">A collection of blob names you are wanting to return.</param>
    /// <param name="policyName">The name of the security policy for the container.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
    /// <returns>A <see cref="IBlobReturnOption{TResult}"/> which has been configured by the generic arguments.</returns>
    Task<IDictionary<string, string>> GetSomeAsync<TResponse>(
        string containerName,
        IEnumerable<string> blobNames,
        string? policyName,
        CancellationToken cancellationToken)
        where TResponse : ReturnProtectedUrlOption, new();

    /// <summary>
    /// Returns all the blob within a container within the storage account.
    /// </summary>
    /// <param name="containerName">The name of the container where the blobs you want to return are stored in.</param>
    /// <param name="policyName">The name of the security policy for the container.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
    /// <returns>A collection of <see cref="IBlobReturnOption{TResult}"/> which has been configured by the generic arguments.</returns>
    Task<IDictionary<string, string>> GetAllAsync<TResponse>(
        string containerName,
        string? policyName,
        CancellationToken cancellationToken)
        where TResponse : ReturnProtectedUrlOption, new();
}
