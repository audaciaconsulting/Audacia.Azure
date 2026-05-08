using System.Threading;
using System.Threading.Tasks;

namespace Audacia.Azure.StorageQueue.DeleteMessageFromQueue;

/// <summary>
/// Delete service for removing messages from an Azure Queue Storage account.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1554:Method contains optional parameter in type hierarchy", Justification = "Allows common pattern of optional cancellation token parameter in interface methods.")]
public interface IDeleteAzureQueueStorageService
{
    /// <summary>
    /// Removing a message from a queue.
    /// </summary>
    /// <param name="queueName">The name of the queue you want to remove a message from.</param>
    /// <param name="messageId">Id of the message which you want to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Bool whether the message was deleted from the queue.</returns>
    Task<bool> ExecuteAsync(string queueName, string messageId, CancellationToken cancellationToken = default);
}