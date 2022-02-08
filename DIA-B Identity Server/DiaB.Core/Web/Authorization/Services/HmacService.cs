using DiaB.Core.Web.Models;
using LazyCache;
using System;
using DiaB.Core.Common.Helpers;

namespace DiaB.Core.Web.Authorization.Services
{
    public class HmacService : IHmacService
    {
        public const int NonceTimeout = 600;

        private readonly IAppCache _cache;

        public HmacService(IAppCache cache)
        {
            _cache = cache;
        }

        public bool ValidateRequest(HmacRequestDto request, string privateKey)
        {
            try
            {
                var nonce = request.Nonce.ToString();

                if (_cache.Get<long>(nonce) > 0)
                {
                    return false;
                }

                if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - request.RequestTime > NonceTimeout)
                {
                    return false;
                }

                if (GetRequestSignature(request, privateKey) != request.Signature)
                {
                    return false;
                }

                _cache.Add(nonce, request.RequestTime, TimeSpan.FromSeconds(NonceTimeout));

                return true;
            }
            catch (Exception)
            {
                // ignore
            }

            return false;
        }

        public string GetRequestSignature(HmacRequestDto request, string privateKey)
        {
            var signature = SecurityHelper.Sha256($"{request.Data}{privateKey}");

            signature = SecurityHelper.Sha256($"{signature}{request.Nonce}");
            signature = SecurityHelper.Sha256($"{signature}{request.RequestTime}");
            signature = SecurityHelper.Sha256($"{signature}{request.PublicKey}");

            return signature;
        }
    }
}
