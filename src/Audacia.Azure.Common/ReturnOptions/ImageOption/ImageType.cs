using System.Runtime.Serialization;

namespace Audacia.Azure.Common.ReturnOptions.ImageOption
{
    public enum ImageType
    {
        [EnumMember(Value = "png")]
        Png,
        [EnumMember(Value = "jpg")]
        Jpg
    }
}