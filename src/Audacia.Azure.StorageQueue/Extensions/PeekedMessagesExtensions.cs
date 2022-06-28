using System.Collections.Generic;
using System.Linq;
using Azure.Storage.Queues.Models;

namespace Audacia.Azure.StorageQueue.Extensions
{
    public static class PeekedMessagesExtensions
    {
        /// <summary>
        /// Returns all the <see cref="PeekedMessage"/> with the <see cref="PeekedMessage.MessageId"/> of <paramref name="messageId"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static IQueryable<QueueMessage> WithMessageId(this IEnumerable<QueueMessage> source, string messageId)
        {
            return source.AsQueryable().Where(peekedMessage => peekedMessage.MessageId == messageId);
        }
    }
}
