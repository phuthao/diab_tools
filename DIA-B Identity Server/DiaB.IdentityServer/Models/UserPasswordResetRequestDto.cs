namespace DiaB.IdentityServer.Models
{
	public class UserPasswordResetRequestDto
	{
		public string PhoneNumber { get; set; }

		public string Password { get; set; }

        public string Token { get; set; }
    }
}
