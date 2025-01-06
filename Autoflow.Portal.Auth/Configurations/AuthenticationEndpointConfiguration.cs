namespace Autoflow.Portal.Auth.Configurations
{
    internal class AuthenticationEndpointConfiguration
    {
        public const string ConfigurationSection = "ClientConfiguration:AuthenticationEndpoint";
        public required string Id { get; set; }
        public required string DisplayName { get; set; }
        public required string RedirectUris { get; set; }
        public required string ServerPath { get; set; }
    }
}
