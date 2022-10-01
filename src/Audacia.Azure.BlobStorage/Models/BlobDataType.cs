using System.Runtime.Serialization;

namespace Audacia.Azure.BlobStorage.Models
{
    /// <summary>
    /// This is all the types of data which a blob can have while using this library.
    /// </summary>
    public enum BlobDataType
    {
        /// <summary>
        /// When blob data is base 64 format.
        /// </summary>
        [EnumMember(Value = "Base 64")]
        BaseSixtyFour,
        
        /// <summary>
        /// When blob data is in the stream format.
        /// </summary>
        Stream,

        /// <summary>
        /// When blob data is in byte array format.
        /// </summary>
        [EnumMember(Value = "Byte Array")]
        ByteArray,

        /// <summary>
        /// When blob data is from a file on the local file system.
        /// </summary>
        [EnumMember(Value = "File Location")]
        FileLocation
    }
}
