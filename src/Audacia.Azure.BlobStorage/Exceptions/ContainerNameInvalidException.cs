namespace Audacia.Azure.BlobStorage.Exceptions
{
    public class ContainerNameInvalidException : Exception
    {
        private const string TemplateExceptionMessage =
            "Cannot {0} a new container {1} with a name that is null / empty";

        private ContainerNameInvalidException(string exceptionMessage) : base(exceptionMessage)
        {
        }

        public static ContainerNameInvalidException UnableToFindWithContainerName(IFormatProvider formatProvider)
        {
            return new ContainerNameInvalidException(string.Format(
                formatProvider,
                TemplateExceptionMessage,
                "Find",
                string.Empty));
        }

        public static ContainerNameInvalidException UnableToCreateWithContainerName(string containerName, IFormatProvider formatProvider)
        {
            return new ContainerNameInvalidException(string.Format(
                formatProvider,
                TemplateExceptionMessage,
                "Create",
                containerName));
        }
    }
}
