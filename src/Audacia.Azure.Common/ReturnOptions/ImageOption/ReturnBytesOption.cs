namespace Audacia.Azure.Common.ReturnOptions.ImageOption
{
    public class ReturnBytesOption : IBlobReturnOption<byte[]>
    {
        private string _blobName;

        /// <summary>
        /// Gets Name of the blob received from the storage account.
        /// </summary>
        public string BlobName => _blobName;

        /// <summary>
        /// Gets the returning value from blob storage containing the base 64 string of the image.
        /// </summary>
#pragma warning disable CA1819
        public byte[] Result { get; private set; }
#pragma warning restore CA1819

        /// <summary>
        /// Returns an array of bytes for the data of the Image from Blob storage.
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="bytes">The bytes of the image returned from blob storage.</param>
        /// <returns>An array bytes which contains the data for the image.</returns>
        public byte[] Parse(string blobName, byte[] bytes, Uri blobClientUrl)
        {
            _blobName = blobName;
            Result = bytes;

            return bytes;
        }
    }
}
