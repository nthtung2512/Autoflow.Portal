namespace Autoflow.Portal.Auth.Configurations
{
    public class AuthenticationEndPoint
    {
        public const string ConfigurationSection = "ClientConfiguration:AuthenticationEndpoint";
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public required string DisplayName { get; set; }
    }
}
