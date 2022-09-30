namespace Audacia.Azure.BlobStorage.Exceptions
{
    /// <summary>
    /// Exception for when the blob container does not exist.
    /// </summary>
    public class ContainerDoesNotExistException : Exception
    {
        private const string TemplateExceptionMessage =
            "Container: {0} does not exist therefore unable to find the blob within the specified container";

        /// <summary>
        /// Exception for when the blob container does not exist.
        /// </summary>
        public ContainerDoesNotExistException()
        {
        }

        /// <summary>
        /// Exception for when the blob container does not exist.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        public ContainerDoesNotExistException(string message) : base(message)
        {
        }

        /// <summary>
        /// Exception for when the blob container does not exist.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">Inner exception thrown.</param>
        public ContainerDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Exception for when the blob container does not exist.
        /// </summary>
        /// <param name="containerName">Name of the blob container which does not exist.</param>
        /// <param name="formatProvider">The format provider.</param>
        public ContainerDoesNotExistException(string containerName, IFormatProvider formatProvider) : base(
            string.Format(formatProvider, TemplateExceptionMessage, containerName))
        {
        }
    }
}
