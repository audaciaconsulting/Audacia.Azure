namespace Audacia.Azure.StorageQueue.Models
{
    /// <summary>
    /// Message object pulled from the queue.
    /// </summary>
    public class AzureQueueStorageMessage
    {
        /// <summary>
        /// Gets the Id of the message received from a queue.
        /// </summary>
        public string MessageId { get; }

        /// <summary>
        /// Gets the receipt that the message has been popped off queue.
        /// </summary>
        public string PopReceipt { get; }

        /// <summary>
        /// Gets the content of the message from the queue.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets when the message was inserted on the queue.
        /// </summary>
        public DateTimeOffset? InsertedOn { get; }

        /// <summary>
        /// Gets when the message has been received.
        /// </summary>
        public DateTime Received { get; }

        /// <summary>
        /// Message object pulled from the queue.
        /// </summary>
        /// <param name="messageId">Id of the message received from a queue.</param>
        /// <param name="popReceipt">The receipt that the message has been popped off queue.</param>
        /// <param name="message">The content of the message from the queue.</param>
        /// <param name="insertedOn">When the message was inserted on the queue.</param>
        /// <param name="received">When the message has been received.</param>
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
