namespace Audacia.Azure.Common.ReturnOptions.ImageOption
{
    /// <summary>
    /// Return type of blob in enumerable of bytes.
    /// </summary>
    public class ReturnBytesOption : IBlobReturnOption<IEnumerable<byte>>
    {
        private string _blobName;

        /// <summary>
        /// Gets Name of the blob received from the storage account.
        /// </summary>
        public string BlobName => _blobName;

        /// <summary>
        /// Gets the returning value from blob storage containing the base 64 string of the image.
        /// </summary>
        public IEnumerable<byte> Result { get; private set; }

        /// <summary>
        /// Returns an array of bytes for the data of the Image from Blob storage.
        /// </summary>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="bytes">The bytes of the image returned from blob storage.</param>
        /// <param name="blobClientUrl">Where the blob is stored in Azure.</param>
        /// <returns>An array bytes which contains the data for the image.</returns>
        public IEnumerable<byte> Parse(string blobName, byte[] bytes, Uri blobClientUrl)
        {
            _blobName = blobName;
            Result = bytes;

            return bytes;
        }
    }
}
