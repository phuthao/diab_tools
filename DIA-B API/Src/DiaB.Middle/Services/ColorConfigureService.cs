using System.Linq;
using DiaB.Common.Constants;
using DiaB.Common.Extensions;
using DiaB.Data.Database.Entities.Common;
using DiaB.Data.Repositories.Interfaces;
using DiaB.Middle.Services.Interfaces;

namespace DiaB.Middle.Services
{
    public class ColorConfigureService : CommonConfigureService, IColorConfigureService
    {
        public ColorConfigureService(IAppRepo<CommonConfigureEntity> repo)
            : base(repo)
        {
        }

        public string GetColorCode(string colorName)
        {
            var pattern = string.Format(CommonConfigurationConstant.TwoKeyPattern, CommonConfigurationConstant.Color, colorName);

            return ColorConfigs.Where(r => r.Key.IsMatch(pattern)).Select(r => r.Value).FirstOrDefault();
        }
    }
}
