namespace Audacia.Azure.StorageQueue.GetMessages.Commands
{
    public class GetMessageStorageQueueCommand
    {
        public string QueueName { get; }

        public bool DeleteMessageAfterReceiving { get; }

        /// <summary>
        ///
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
