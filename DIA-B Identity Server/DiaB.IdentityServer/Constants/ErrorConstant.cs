using System.Collections.Generic;

namespace DiaB.IdentityServer.Constants
{
    public static class ErrorConstant
    {
        public static KeyValuePair<string, string> UserNotFound = new KeyValuePair<string, string>("USER001", "User does not exist.");

        public static KeyValuePair<string, string> UserDuplicated = new KeyValuePair<string, string>("USER002", "User was existed.");

        public static KeyValuePair<string, string> UserVerified = new KeyValuePair<string, string>("USER003", "User's already verified.");

        public static KeyValuePair<string, string> PhoneNumberExists = new KeyValuePair<string, string>("USER004", "Phone number exists.");

        public static KeyValuePair<string, string> ExternalUserExists = new KeyValuePair<string, string>("USER005", "External user exists.");

        public static KeyValuePair<string, string> InvalidToken = new KeyValuePair<string, string>("TOKEN001", "The inputted token is invalid.");

        public static KeyValuePair<string, string> ShortPassword = new KeyValuePair<string, string>("PASS001", "Passwords must be at least {0} characters.");

        public static KeyValuePair<string, string> InvalidPassword = new KeyValuePair<string, string>("PASS002", "Invalid password.");

        public static KeyValuePair<string, string> CannotUnlink = new KeyValuePair<string, string>("External001", "Cannot unlink external account.");
    }
}
