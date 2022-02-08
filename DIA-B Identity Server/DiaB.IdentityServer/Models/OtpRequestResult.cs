namespace DiaB.IdentityServer.Models
{
    public class OtpRequestResult
    {
        public bool IsSuccess { get; set; }

        public string Token { get; set; }

        public int RemainingRequestCount { get; set; }

        public string Message { get; set; }
    }
}
