namespace Audacia.Azure.StorageQueue.GetMessages.Commands
{
    /// <summary>
    /// Command for getting a message from a storage queue and removing it after receiving.
    /// </summary>
    public class GetWithCleanUpMessageStorageQueueCommand
    {
        /// <summary>
        /// Gets the Name of the queue.
        /// </summary>
        public string QueueName { get; }

        /// <summary>
        /// Gets a value indicating whether the message is to be delete after received.
        /// </summary>
        public bool ShouldDeleteMessageAfterReceiving { get; }

        /// <summary>
        /// Command for getting a message from a storage queue and removes it.
        /// </summary>
        /// <param name="queueName">Name of queue which you want to get the messages from.</param>
        public GetWithCleanUpMessageStorageQueueCommand(string queueName)
        {
            QueueName = queueName;
            ShouldDeleteMessageAfterReceiving = true;
        }
    }
}
