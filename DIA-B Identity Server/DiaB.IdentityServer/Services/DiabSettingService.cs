using System.Linq;
using DiaB.IdentityServer.Database;

namespace DiaB.IdentityServer.Services
{
    public class DiabSettingService : IDiabSettingService
    {
        private readonly DiabDbContext _dbContext;

        public DiabSettingService(DiabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GetHotLine()
        {
            var result = _dbContext.DiabSettings.FirstOrDefault(s => s.Key == "DiaB.Information.Contact.Hotline");

            return result?.Value;
        }
    }
}