using System.Collections.Generic;
using CpTech.Core.Middle.Services;

namespace DiaB.Middle.Services.Interfaces
{
    public interface ICommonConfigureService : ICoreService
    {
        Dictionary<string, string> ColorConfigs { get; set; }
    }
}
