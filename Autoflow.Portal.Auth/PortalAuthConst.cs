namespace Autoflow.Portal.Auth
{
    internal static class PortalAuthConst
    {
        public const string TokenEndpoint = "connect/token";
        public const string AuthorizeEndpoint = "connect/authorization";
        public const string UserInfoEndpoint = "connect/userinfo";
        public const string LogoutEndpoint = "connect/logout";

        public const string Username = "username";
        public const string Password = "password";

        public static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromHours(1);
        public static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(7);
        public static readonly TimeSpan AuthorizationCodeLifetime = TimeSpan.FromMinutes(3);

        internal static class CustomClaims
        {
            public const string Organization = "organization";
        }
    }
}
