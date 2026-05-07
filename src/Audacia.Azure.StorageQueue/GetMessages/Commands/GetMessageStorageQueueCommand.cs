namespace Audacia.Azure.StorageQueue.GetMessages.Commands;

/// <summary>
/// Command for getting a message from a storage queue and does not remove it.
/// </summary>
/// <param name="queueName">Name of queue which you want to get the messages from.</param>
public class GetMessageStorageQueueCommand(string queueName)
{
    /// <summary>
    /// Gets the Name of the queue.
    /// </summary>
    public string QueueName { get; } = queueName;

    /// <summary>
    /// Gets a value indicating whether the message is to be delete after received.
    /// </summary>
    public bool ShouldDeleteMessageAfterReceiving { get; } = false;
}
