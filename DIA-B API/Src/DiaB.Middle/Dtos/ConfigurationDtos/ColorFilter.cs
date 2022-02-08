using DiaB.Common.Constants;

namespace DiaB.Middle.Dtos.ConfigurationDtos
{
    public partial class ConfigurationDtos
    {
        public partial class ColorFilter
        {
            public override string KeyPattern => string.Format(CommonConfigurationConstant.OneKeyPattern, CommonConfigurationConstant.Color);
        }
    }
}
