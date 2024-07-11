namespace Autoflow.Portal.Domain.Shared
{
    public class ClientInfo
    {
        public string ClientId { get; set; } = string.Empty;
        public Guid ClientSecret { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public Guid? OrganizationId { get; set; }
    }
}
