namespace Audacia.Azure.Common.ReturnOptions.ImageOption
{
    public class ReturnBaseSixtyFourOption : IBlobReturnOption<string>
    {
        private string _blobName;

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
        public string Result { get; private set; }

        /// <summary>
        /// Converts the bytes from blob storage to a base 64 string.
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="bytes"></param>
        /// <param name="blobClientUrl"></param>
        /// <returns></returns>
        public string Parse(string blobName, byte[] bytes, Uri blobClientUrl)
        {
            _blobName = blobName;
            var baseSixtyFourString = Convert.ToBase64String(bytes);

            Result = $"data:image/{Type.ToString().ToUpperInvariant()};base64,{baseSixtyFourString}";

            return Result;
        }
    }
}
