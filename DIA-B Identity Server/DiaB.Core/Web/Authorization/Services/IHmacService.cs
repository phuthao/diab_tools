using DiaB.Core.Web.Models;

namespace DiaB.Core.Web.Authorization.Services
{
    public interface IHmacService
    {
        bool ValidateRequest(HmacRequestDto request, string privateKey);

        string GetRequestSignature(HmacRequestDto request, string privateKey);
    }
}
