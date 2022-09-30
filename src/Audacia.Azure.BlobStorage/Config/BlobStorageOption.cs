namespace Audacia.Azure.BlobStorage.Config
{
    /// <summary>
    /// Config class used for added the relevant config options needed to connect Azure blob storage account.
    /// </summary>
    public class BlobStorageOption
    {
        /// <summary>
        /// Location of the Azure Blob storage config in appsettings.json.
        /// </summary>
        public const string OptionConfigLocation = "Azure:BlobStorageConfig";

        /// <summary>
        /// Gets or sets the name of the Azure Blob storage account.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the key of the Azure Blob Storage account.
        /// </summary>
        public string AccountKey { get; set; }
    }
}
