using System.Collections.Generic;
using System.Linq;
using Azure.Storage.Blobs.Models;

namespace Audacia.Azure.BlobStorage.Extensions
{
    /// <summary>
    /// Extension class for all collections of <see cref="BlobContainerItem"/>.
    /// </summary>
    public static class BlobContainerItemsExtensions
    {
        /// <summary>
        /// Checks if the new proposed container name already existing within the storage account.
        /// </summary>
        /// <param name="source">The existing blob containers within the storage account.</param>
        /// <param name="newContainerName">New container name.</param>
        /// <returns>Whether the new container name exists within the current storage account.</returns>
        public static bool DoesBlobAlreadyExists(this IEnumerable<BlobContainerItem> source, string newContainerName)
        {
            return source.Any(container => container.Name == newContainerName);
        }
    }
}
