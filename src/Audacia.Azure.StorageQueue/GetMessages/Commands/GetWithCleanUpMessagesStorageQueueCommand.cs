namespace Audacia.Azure.StorageQueue.GetMessages.Commands;

/// <summary>
/// Command for getting messages from a storage queue and removes it.
/// </summary>
/// <param name="queueName">Name of queue which you want to get the messages from.</param>
/// <param name="amountToReceive">The amount of messages you want to get from the queue.</param>
public class GetWithCleanUpMessagesStorageQueueCommand(string queueName, int amountToReceive)
{
    /// <summary>
    /// Gets the Name of the queue.
    /// </summary>
    public string QueueName { get; } = queueName;

    /// <summary>
    /// Gets the number of messages to receive.
    /// </summary>
    public int AmountToReceive { get; } = amountToReceive;

    /// <summary>
    /// Gets a value indicating whether the message is to be delete after received.
    /// </summary>
    public bool ShouldDeleteMessageAfterReceiving { get; } = true;
}
