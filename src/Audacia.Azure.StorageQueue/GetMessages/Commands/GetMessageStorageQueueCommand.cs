namespace Audacia.Azure.StorageQueue.GetMessages.Commands
{
    /// <summary>
    /// Command for getting a message from a storage queue.
    /// </summary>
    public class GetMessageStorageQueueCommand
    {
        /// <summary>
        /// Gets the Name of the queue.
        /// </summary>
        public string QueueName { get; }

        /// <summary>
        /// Gets a value indicating whether the message is to be delete after received.
        /// </summary>
        public bool DeleteMessageAfterReceiving { get; }

        /// <summary>
        /// Command for getting a message from a storage queue.
        /// </summary>
        /// <param name="queueName">Name of queue which you want to get the messages from.</param>
        /// <param name="deleteMessageAfterReceiving">
        /// Whether you want to remove the messages from the queue after they have been received.
        /// </param>
        public GetMessageStorageQueueCommand(string queueName, bool deleteMessageAfterReceiving = true)
        {
            QueueName = queueName;
            DeleteMessageAfterReceiving = deleteMessageAfterReceiving;
        }
    }
}
