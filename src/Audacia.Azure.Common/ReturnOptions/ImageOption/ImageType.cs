using System.Runtime.Serialization;

namespace Audacia.Azure.Common.ReturnOptions.ImageOption
{
    /// <summary>
    /// The type of images which is currently supported.
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// Enum value for if image is unknown.
        /// </summary>
        [EnumMember(Value = "None")] 
        None = 0,
        
        /// <summary>
        /// Enum value for if image is PNG.
        /// </summary>
        [EnumMember(Value = "png")] 
        Png = 100,

        /// <summary>
        /// Enum value for if image is JPG.
        /// </summary>
        [EnumMember(Value = "jpg")] 
        Jpg = 200
    }
}
