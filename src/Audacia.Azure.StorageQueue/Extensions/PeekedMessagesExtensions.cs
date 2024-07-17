using System.Collections.Generic;
using System.Linq;
using Azure.Storage.Queues.Models;

namespace Audacia.Azure.StorageQueue.Extensions
{
    /// <summary>
    /// All extension methods for a collection of Peeked messages.
    /// </summary>
    public static class PeekedMessagesExtensions
    {
        /// <summary>
        /// Returns all the <see cref="QueueMessage"/> with the <see cref="QueueMessage.MessageId"/> of <paramref name="messageId"/>.
        /// </summary>
        /// <param name="source">IEnumerable of <see cref="QueueMessage"/>.</param>
        /// <param name="messageId">Id of an <see cref="QueueMessage"/>.</param>
        /// <returns>Filtered collection of <see cref="QueueMessage"/>.</returns>
        public static IEnumerable<QueueMessage> WithMessageId(this IEnumerable<QueueMessage> source, string messageId)
        {
            return source.Where(peekedMessage => peekedMessage.MessageId == messageId).ToList();
        }
    }
}
