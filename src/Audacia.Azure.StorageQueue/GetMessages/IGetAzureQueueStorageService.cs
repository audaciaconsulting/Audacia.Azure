using System.Collections.Generic;
using System.Threading.Tasks;
using Audacia.Azure.StorageQueue.GetMessages.Commands;
using Audacia.Azure.StorageQueue.Models;

namespace Audacia.Azure.StorageQueue.GetMessages
{
    /// <summary>
    /// Get service for returning messages from an Azure Queue Storage account.
    /// </summary>
    public interface IGetAzureQueueStorageService
    {
        /// <summary>
        /// Returns the top message from a queue.
        /// </summary>
        /// <param name="command">
        /// Containing information of the queue name and whether to delete the message from storage after it has been received.
        /// </param>
        /// <returns>The message from the queue if there is one.</returns>
        Task<AzureQueueStorageMessage> GetAsync(GetMessageStorageQueueCommand command);

        /// <summary>
        /// Return a custom amount of messages from a queue.
        /// </summary>
        /// <param name="command">
        /// Command containing the queue name, the amount of messages to get and whether to delete the messages after
        /// they have been pulled from storage.
        /// </param>
        /// <returns>
        /// A collection of size <paramref name="command.AmountToReceive"/> of <see cref="AzureQueueStorageMessage"/>'s if
        /// available if not the remaining amount that are currently in the queue.
        /// </returns>
        Task<IEnumerable<AzureQueueStorageMessage>> GetSomeAsync(GetMessagesStorageQueueCommand command);
    }
}
