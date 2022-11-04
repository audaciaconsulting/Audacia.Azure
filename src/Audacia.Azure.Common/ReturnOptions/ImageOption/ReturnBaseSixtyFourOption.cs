namespace Audacia.Azure.Common.ReturnOptions.ImageOption
{
    /// <summary>
    /// Return type of blob in base 64 format.
    /// </summary>
    public class ReturnBaseSixtyFourOption : IBlobReturnOption<string>
    {
        private string _blobName = default!;

        /// <summary>
        /// Gets Name of the blob received from the storage account.
        /// </summary>
        public string BlobName => _blobName;

        /// <summary>
        /// Gets or Sets The image type will default to <see cref="ImageType.Png"/>.
        /// </summary>
        public ImageType Type { get; set; } = ImageType.Png;

        /// <summary>
        /// Gets the returning value from blob storage containing the base 64 string of the image.
        /// </summary>
        public string Result { get; private set; } = default!;

        /// <summary>
        /// Converts the bytes from blob storage to a base 64 string.
        /// </summary>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="bytes">Array of bytes representing the blob data.</param>
        /// <param name="blobClientUrl">URL of the blob client.</param>
        /// <returns>Returns a base 64 string for the data of the blob.</returns>
        public string Parse(string blobName, byte[] bytes, Uri blobClientUrl)
        {
            _blobName = blobName;
            var baseSixtyFourString = Convert.ToBase64String(bytes);

            Result = $"data:image/{Type.ToString().ToUpperInvariant()};base64,{baseSixtyFourString}";

            return Result;
        }
    }
}
