namespace Audacia.Azure.StorageQueue.Config
{
    /// <summary>
    /// Config class used for added the relevant config options needed to connect Azure queue storage account.
    /// </summary>
    public class QueueStorageOption
    {
        /// <summary>
        /// Location of the Azure Storage queue config in appsettings.json.
        /// </summary>
        public const string OptionConfigLocation = "Azure:QueueStorageConfig";

        /// <summary>
        /// Gets or sets the name of the Azure Blob storage account.
        /// </summary>
        public string AccountName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the key of the Azure Blob Storage account.
        /// </summary>
        public string AccountKey { get; set; } = default!;
    }
}
