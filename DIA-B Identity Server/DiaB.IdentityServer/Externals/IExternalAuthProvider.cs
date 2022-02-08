using Newtonsoft.Json.Linq;

namespace DiaB.IdentityServer.Externals
{
    public interface IExternalAuthProvider
    {
        JObject GetUserInfo(string accessToken);
    }
}
