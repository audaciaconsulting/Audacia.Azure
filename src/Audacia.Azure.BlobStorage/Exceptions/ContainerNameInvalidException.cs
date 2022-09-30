namespace Audacia.Azure.BlobStorage.Exceptions
{
    /// <summary>
    /// Exception for when the blob container name is invalid.
    /// </summary>
    public class ContainerNameInvalidException : Exception
    {
        private const string TemplateExceptionMessage =
            "Cannot {0} a new container {1} with a name that is null / empty";

        /// <summary>
        /// Exception for when the blob container name is invalid.
        /// </summary>
        public ContainerNameInvalidException()
        {
        }

        /// <summary>
        /// Exception for when the blob container name is invalid.
        /// </summary>
        /// <param name="exceptionMessage">Message of the exception.</param>
        private ContainerNameInvalidException(string exceptionMessage) : base(exceptionMessage)
        {
        }

        /// <summary>
        /// Exception for when the blob container name is invalid.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">The format provider.</param>
        public ContainerNameInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Exception for when the blob container name is invalid.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>Instance of <see cref="ContainerNameInvalidException"/>.</returns>
        public static ContainerNameInvalidException UnableToFindWithContainerName(IFormatProvider formatProvider)
        {
            return new ContainerNameInvalidException(string.Format(
                formatProvider,
                TemplateExceptionMessage,
                "Find",
                string.Empty));
        }

        /// <summary>
        /// Exception for when the blob container name is invalid.
        /// </summary>
        /// <param name="containerName">Name of the blob container which does not exist.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>Instance of <see cref="ContainerNameInvalidException"/>.</returns>
        public static ContainerNameInvalidException UnableToCreateWithContainerName(
            string containerName,
            IFormatProvider formatProvider)
        {
            return new ContainerNameInvalidException(string.Format(
                formatProvider,
                TemplateExceptionMessage,
                "Create",
                containerName));
        }
    }
}
