using DiaB.Common.Enums;

namespace DiaB.Middle.Dtos.ConfigurationDtos
{
    public partial class ConfigurationDtos
    {
        public partial class Filter
        {
            public string Key { get; set; }

            public string Value { get; set; }

            public ConfigureSetupTypes? ConfigureSetupType { get; set; }
        }
    }
}
