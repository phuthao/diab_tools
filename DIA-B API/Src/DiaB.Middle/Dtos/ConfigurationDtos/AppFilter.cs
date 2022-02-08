using System.ComponentModel;
using DiaB.Common.Enums;

namespace DiaB.Middle.Dtos.ConfigurationDtos
{
    public partial class ConfigurationDtos
    {
        public partial class AppFilter
        {
            [Description("Key")]
            public string Key { get; set; }

            [Description("Value")]
            public string Value { get; set; }

            [Description("Sắp xếp")]
            public string OrderBy { get; set; }

            public ConfigureSetupTypes? ConfigureSetupType { get; set; }
        }
    }
}
