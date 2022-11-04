namespace Audacia.Azure.StorageQueue.GetMessages.Commands;

/// <summary>
/// Command for getting messages from a storage queue and removes it.
/// </summary>
public class GetWithCleanUpMessagesStorageQueueCommand
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
    /// Command for getting messages from a storage queue and removes it.
    /// </summary>
    /// <param name="queueName">Name of queue which you want to get the messages from.</param>
    /// <param name="amountToReceive">The amount of messages you want to get from the queue.</param>
    public GetWithCleanUpMessagesStorageQueueCommand(string queueName, int amountToReceive)
    {
        QueueName = queueName;
        AmountToReceive = amountToReceive;
        ShouldDeleteMessageAfterReceiving = true;
    }
}
