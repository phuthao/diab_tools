using System.Text.RegularExpressions;

namespace DiaB.IdentityServer.Helpers
{
    public class PhoneNumberHelper
    {
        public static string GetFormatedPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return default;
            }

            var value = phoneNumber.Replace("-", string.Empty);

            if (value.StartsWith("0"))
            {
                var regex = new Regex("0");

                return regex.Replace(value, "+84", 1);
            }

            if (!value.StartsWith("+84"))
            {
                return "+84" + value;
            }

            return value;
        }
    }
}
