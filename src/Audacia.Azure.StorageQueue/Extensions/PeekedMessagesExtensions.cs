using Azure.Storage.Queues.Models;

namespace Audacia.Azure.StorageQueue.Extensions
{
    public static class PeekedMessagesExtensions
    {
        /// <summary>
        /// Returns all the <see cref="QueueMessage"/> with the <see cref="QueueMessage.MessageId"/> of <paramref name="messageId"/>.
        /// </summary>
        /// <param name="source">IEnumerable of <see cref="QueueMessage"/>.</param>
        /// <param name="messageId">Id of an <see cref="QueueMessage"/>.</param>
        /// <returns>Filtered collection of <see cref="QueueMessage"/>.</returns>
        public static ICollection<QueueMessage> WithMessageId(this IEnumerable<QueueMessage> source, string messageId)
        {
            return source.AsQueryable().Where(peekedMessage => peekedMessage.MessageId == messageId).ToList();
        }
    }
}
