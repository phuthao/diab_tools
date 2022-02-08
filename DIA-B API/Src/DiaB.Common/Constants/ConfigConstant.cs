namespace DiaB.Common.Constants
{
    public static class ConfigConstant
    {
        public const string Configs = nameof(Configs);
        public const string DevelopmentEnv = "Development";
        public const string LocalEnv = "local";
        public const string Url = nameof(Url);
        public const string Domain = nameof(Domain);

        public const string ASPNETCOREENV = "ASPNETCORE_ENVIRONMENT";
        public const string CommonConfigsRelativePath = "../DiaB.Common/Configs";
        public const string DefaultAppSetting = "appsettings.json";
        public const string AppSettingByEnvironment = "appsettings.{0}.json";

        public const string AppSettingIdentityAuthority = "Identity:Authority";
        public const string AppSettingIdentityAudience = "Identity:Audience";
        public const string AppSettingFileSecret = "File:Secret";

        public const string AccountToken = "account-token";

        public const string InternalScope = "Internal";

        public const string ApiUser = "api/Admin/v1/users/{0}/{1}";
        public const string IsCreateUserApi = "api/Admin/v1/users";
        public const string IsResetPasswordApi = "api/Admin/v1/users/{0}/reset-password";
        public const string IsUserStatusApi = "api/Admin/v1/user-status";
        public const string IsUserPhoneNumberApi = "api/account/v1/users/current/phone-number";
    }
}
