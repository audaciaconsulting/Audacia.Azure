using System;

namespace Audacia.Azure.StorageQueue.Exceptions
{
    /// <summary>
    /// Exception for when the Queue within Azure does not exist.
    /// </summary>
#pragma warning disable CA1032
    public class QueueDoesNotExistException : Exception
#pragma warning restore CA1032
    {
        private const string TemplateExceptionMessage = "There is no queue with the name: {0}";

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
}
