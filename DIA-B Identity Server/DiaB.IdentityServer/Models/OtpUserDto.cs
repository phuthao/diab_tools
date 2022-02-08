namespace DiaB.IdentityServer.Models
{
	public class OtpUserDto
	{
		public UserDto User { get; set; }

		public string Token { get; set; }
	}
}
