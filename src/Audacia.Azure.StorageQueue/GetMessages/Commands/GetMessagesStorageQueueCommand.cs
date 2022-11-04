namespace Audacia.Azure.StorageQueue.GetMessages.Commands
{
    /// <summary>
    /// Command for getting messages from a storage queue.
    /// </summary>
    public class GetMessagesStorageQueueCommand
    {
        /// <summary>
        /// Gets the Name of the queue.
        /// </summary>
        public string QueueName { get; }

        /// <summary>
        /// Gets the number of messages to receive.
        /// </summary>
        public int AmountToReceive { get; }

        /// <summary>
        /// Gets a value indicating whether the message is to be delete after received.
        /// </summary>
        public bool ShouldDeleteMessageAfterReceiving { get; }

        /// <summary>
        /// Command for getting messages from a storage queue.
        /// </summary>
        /// <param name="queueName">Name of queue which you want to get the messages from.</param>
        /// <param name="amountToReceive">The amount of messages you want to get from the queue.</param>
        /// <param name="shouldDeleteMessageAfterReceiving">
        /// Whether you want to remove the messages from the queue after they have been received.
        /// </param>
        public GetMessagesStorageQueueCommand(
            string queueName,
            int amountToReceive,
            bool shouldDeleteMessageAfterReceiving = true)
        {
            QueueName = queueName;
            AmountToReceive = amountToReceive;
            ShouldDeleteMessageAfterReceiving = shouldDeleteMessageAfterReceiving;
        }
    }
}
