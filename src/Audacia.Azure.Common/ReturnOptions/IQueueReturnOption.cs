namespace Audacia.Azure.Common.ReturnOptions
{
    /// <summary>
    /// Generic interface for return storage queue option.
    /// </summary>
    /// <typeparam name="T">Generic parameter determining the type of the parsed storage queue data.</typeparam>
    public interface IQueueReturnOption<T>
    {
        /// <summary>
        /// Gets or sets the data of the storage queue message.
        /// </summary>
        T Result { get; set; }

        /// <summary>
        /// Trys and parses the storage queue message into the generic type.
        /// </summary>
        /// <param name="jsonString">JSON string from the storage queue message.</param>
        /// <returns>Parsed value of type <ref name="T"/>.</returns>
        T Parse(string jsonString);
    }
}
