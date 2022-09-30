using Newtonsoft.Json;

namespace Audacia.Azure.Common.ReturnOptions.ClassOption
{
    /// <summary>
    /// Returning a strongly typed blob.
    /// </summary>
    /// <typeparam name="T">Typing for return blob option.</typeparam>
    public class BlobReturnClassOption<T> : IQueueReturnOption<T>
    {
        /// <summary>
        /// Gets or sets the value of the result.
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Trys parses the return value from blob storage account to a strongly typed value.
        /// </summary>
        /// <param name="jsonString">String of JSON representing the data.</param>
        /// <returns>A value of the generic type.</returns>
        /// <exception cref="ArgumentNullException">If the JSON string is null from Azure Blob storage.</exception>
        public T Parse(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                throw new ArgumentNullException(
                    jsonString,
                    "Json string is either null or empty and therefore cannot be deserialized into a object.");
            }

            return JsonConvert.DeserializeObject<T>(jsonString)!;
        }
    }
}
