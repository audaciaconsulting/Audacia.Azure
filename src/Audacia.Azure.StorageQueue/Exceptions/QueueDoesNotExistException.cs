using System;

namespace Audacia.Azure.StorageQueue.Exceptions;

/// <summary>
/// Exception for when the Queue within Azure does not exist.
/// </summary>
public class QueueDoesNotExistException : Exception
{
    private const string TemplateExceptionMessage = "There is no queue with the name: {0}";

    /// <summary>
    /// Initializes a new instance of the <see cref="QueueDoesNotExistException"/> class.
    /// </summary>
    public QueueDoesNotExistException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueueDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public QueueDoesNotExistException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueueDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public QueueDoesNotExistException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueueDoesNotExistException"/> class.
    /// </summary>
    /// <param name="queueName">Name of the queue which doesn't exist.</param>
    /// <param name="formatProvider">Format provider for formatting the exception message.</param>
    public QueueDoesNotExistException(string queueName, IFormatProvider formatProvider) : base(
        string.Format(formatProvider, TemplateExceptionMessage, queueName))
    {
    }
}
