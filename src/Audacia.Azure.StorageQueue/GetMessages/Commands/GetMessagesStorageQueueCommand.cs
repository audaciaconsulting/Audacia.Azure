namespace Audacia.Azure.StorageQueue.GetMessages.Commands
{
    public class GetMessagesStorageQueueCommand
    {
        public string QueueName { get; }

        public int AmountToReceive { get; }

        public bool DeleteMessageAfterReceiving { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="queueName">Name of queue which you want to get the messages from.</param>
        /// <param name="amountToReceive">The amount of messages you want to get from the queue.</param>
        /// <param name="deleteMessageAfterReceiving">
        /// Whether you want to remove the messages from the queue after they have been received.
        /// </param>
        public GetMessagesStorageQueueCommand(string queueName, int amountToReceive,
            bool deleteMessageAfterReceiving = true)
        {
            QueueName = queueName;
            AmountToReceive = amountToReceive;
            DeleteMessageAfterReceiving = deleteMessageAfterReceiving;
        }
    }
}
