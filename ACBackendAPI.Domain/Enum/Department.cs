using System.ComponentModel;

namespace ACBackendAPI.Domain.Enum
{
    public enum Department
    {
        [Description("Science Department")]
        Science = 1,

        [Description("Art Department")]
        Art,

        [Description("Commercial Department")]
        Commercial
    }
}
