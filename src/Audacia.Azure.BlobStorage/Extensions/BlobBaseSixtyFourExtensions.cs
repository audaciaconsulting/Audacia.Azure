using Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions;
using Audacia.Azure.BlobStorage.Models;

namespace Audacia.Azure.BlobStorage.Extensions;

/// <summary>
/// All extension methods for blob services that is in format of base 64.
/// </summary>
public static class BlobBaseSixtyFourExtensions
{
    /// <summary>
    /// Checks if the file is in valid Base64.
    /// </summary>
    /// <param name="baseSixtyFourBlobData">The data of the blob in Base64.</param>
    /// <param name="blobName">BlobName is the name of the blob in Azure Blob Storage.</param>
    /// <param name="formatProvider">The type of formatter to use for the exception message.</param>
    /// <returns>Returuns the base64 blobdata in bytes.</returns>
    /// <exception cref="BlobDataCannotBeNullException">NullException message.</exception>
    /// <exception cref="BlobDataCannotBeEmptyException">Can't be empty message.</exception>
    /// <exception cref="BlobDataCannotBeInvalidBaseSixtyFourException">Invalid base64 message.</exception>
    public static IEnumerable<byte> BaseSixtyFourBlobChecks(this string baseSixtyFourBlobData, string blobName, IFormatProvider formatProvider)
    {
        if (baseSixtyFourBlobData == null)
        {
            throw new BlobDataCannotBeNullException(blobName, BlobDataType.BaseSixtyFour, formatProvider);
        }

        if (baseSixtyFourBlobData.Length == 0)
        {
            throw new BlobDataCannotBeEmptyException(blobName, BlobDataType.BaseSixtyFour, formatProvider);
        }

        var buffer = new Span<byte>(new byte[baseSixtyFourBlobData.Length]);
        var isValidBaseSixtyFour = Convert.TryFromBase64String(baseSixtyFourBlobData, buffer, out _);

        if (!isValidBaseSixtyFour)
        {
            throw new BlobDataCannotBeInvalidBaseSixtyFourException(
                blobName,
                baseSixtyFourBlobData,
                formatProvider);
        }

        return Convert.FromBase64String(baseSixtyFourBlobData);
    }
}
