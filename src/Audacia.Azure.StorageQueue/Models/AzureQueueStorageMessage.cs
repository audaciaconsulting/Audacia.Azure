using System;

namespace Audacia.Azure.StorageQueue.Models
{
    /// <summary>
    /// Message object pulled from the queue.
    /// </summary>
    public class AzureQueueStorageMessage
    {
        public string MessageId { get; }

        public string PopReceipt { get; }

        public string Message { get; }

        public DateTimeOffset? InsertedOn { get; }

        public DateTime Received { get; }

#pragma warning disable ACL1003
        public AzureQueueStorageMessage(
            string messageId,
            string popReceipt,
            string message,
            DateTimeOffset? insertedOn,
            DateTime received)
#pragma warning restore ACL1003
        {
            MessageId = messageId;
            PopReceipt = popReceipt;
            Message = message;
            InsertedOn = insertedOn;
            Received = received;
        }
    }
}
