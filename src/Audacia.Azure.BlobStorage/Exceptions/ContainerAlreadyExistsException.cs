namespace Audacia.Azure.BlobStorage.Exceptions
{
    /// <summary>
    /// Exception for when the Blob container already exists but expects to create the container.
    /// </summary>
    public class ContainerAlreadyExistsException : Exception
    {
        private const string TemplateExceptionMessage =
            "There is already a container on this storage account with the name: {0}";

        /// <summary>
        /// Exception for when the Blob container already exists but expects to create the container.
        /// </summary>
        public ContainerAlreadyExistsException()
        {
        }

        /// <summary>
        /// Exception for when the Blob container already exists but expects to create the container.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        public ContainerAlreadyExistsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Exception for when the Blob container already exists but expects to create the container.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">Inner exception thrown.</param>
        public ContainerAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Exception for when the Blob container already exists but expects to create the container.
        /// </summary>
        /// <param name="containerName">Name of the container that already exists.</param>
        /// <param name="formatProvider">The format provider.</param>
        public ContainerAlreadyExistsException(string containerName, IFormatProvider formatProvider) : base(
            string.Format(formatProvider, TemplateExceptionMessage, containerName))
        {
        }
    }
}
