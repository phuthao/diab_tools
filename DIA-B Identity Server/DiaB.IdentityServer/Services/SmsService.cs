using DiaB.IdentityServer.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiaB.IdentityServer.Services
{
    public class SmsService : ISmsService
    {
        private readonly SmsSettings _smsSettings;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;

        public SmsService(IOptions<SmsSettings> options, IHttpClientFactory clientFactory, ILogger<SmsService> logger)
        {
            _smsSettings = options.Value;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task SendMessage(string phoneNumber, string token)
        {
            var requestObject = new
            {
                from = _smsSettings.From,
                u = _smsSettings.U,
                pwd = _smsSettings.Pwd,
                phone = phoneNumber,
                sms = string.Format(_smsSettings.Sms, token),
                json = 1
            };

            var httpClient = _clientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(requestObject), Encoding.UTF8, "application/json");

            await httpClient.PostAsync(_smsSettings.ApiUrl, content);

            //_logger.LogWarning($"Phone number: {phoneNumber}, message: {token}");
        }
    }
}
