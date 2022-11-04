using System.Runtime.Serialization;

namespace Audacia.Azure.BlobStorage.Models
{
    /// <summary>
    /// This is all the types of data which a blob can have while using this library.
    /// </summary>
    public enum BlobDataType
    {
        /// <summary>
        /// Enum value for if blob data is unknown.
        /// </summary>
        [EnumMember(Value = "None")] 
        None = 0,

        /// <summary>
        /// When blob data is base 64 format.
        /// </summary>
        [EnumMember(Value = "Base 64")] 
        BaseSixtyFour = 100,

        /// <summary>
        /// When blob data is in the stream format.
        /// </summary>
        Stream = 200,

        /// <summary>
        /// When blob data is in byte array format.
        /// </summary>
        [EnumMember(Value = "Byte Array")]
        ByteArray = 300,

        /// <summary>
        /// When blob data is from a file on the local file system.
        /// </summary>
        [EnumMember(Value = "File Location")]
        FileLocation = 400
    }
}
