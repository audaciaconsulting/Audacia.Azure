using System;

namespace Audacia.Azure.Common.ReturnOptions
{
    /// <summary>
    /// Generic interface for return blob option.
    /// </summary>
    /// <typeparam name="T">Generic parameter determining the type of the parsed blob data.</typeparam>
    public interface IBlobReturnOption<T>
    {
        /// <summary>
        /// Gets the name of the blob.
        /// </summary>
        string BlobName { get; }

        /// <summary>
        /// Gets the data of the blob.
        /// </summary>
        T Result { get; }

        /// <summary>
        /// Parses the data of the blob into the generic parameter type.
        /// </summary>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="bytes">Byte array representing the data of the blob.</param>
        /// <param name="blobClientUrl">URL of the blob client.</param>
        /// <returns>Parsed value in the type the same of the generic type.</returns>
        T Parse(string blobName, byte[] bytes, Uri blobClientUrl);
    }
}
