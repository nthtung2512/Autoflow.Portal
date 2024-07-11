using Autoflow.Portal.Domain.Organization;
using Autoflow.Portal.Domain.Shared;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;

namespace Autoflow.Portal.EntityFrameworkCore
{
    public class Seed
    {
        private readonly PortalDbContext _context;

        public Seed(PortalDbContext context)
        {
            _context = context;
        }

        public void Populate()
        {
            _context.Database.EnsureCreated();
            if (_context.Organizations.Any())
                return;

            var clients = new ClientInfo[]
            {
                new() {ClientId = "chathub", ClientSecret = new Guid("293f9bac-8cb1-4b59-8756-1b5193526980"), DisplayName = "ChatHub", Role = "User", OrganizationId = new Guid("3c7f6db4-5180-4d11-b82b-27c0b1c94b5a")},
                new() {ClientId = "chathub1", ClientSecret = new Guid("d0270316-b546-4c2d-9726-ff2c1eb7f25d"), DisplayName = "ChatHub1", Role = "User", OrganizationId = new Guid("3c7f6db4-5180-4d11-b82b-27c0b1c94b5a")},
                new() {ClientId = "chathub2", ClientSecret = new Guid("594f2eef-5d96-47ab-9e35-062934c68d4b"), DisplayName = "ChatHub2", Role = "User", OrganizationId = new Guid("65bd235a-e416-45e3-b6ea-9bcde89fca43")},
                new() {ClientId = "chathubAdmin", ClientSecret = new Guid("60602bff-f912-4a6d-8ba1-c2f5b543cd71"), DisplayName = "ChatAdmin", Role = "Admin"},
            };

            var organizations = new Organization[]
            {
                new(new Guid("3c7f6db4-5180-4d11-b82b-27c0b1c94b5a")) {OrganizationName = "Organization1", OrganizationMessages = new List<string> {"Welcome to Organization1"}},
                new(new Guid("65bd235a-e416-45e3-b6ea-9bcde89fca43")) { OrganizationName = "Organization2", OrganizationMessages = new List < string > { "Welcome to Organization2" } }
            };

            _context.AddRange(clients);
            _context.AddRange(organizations);
            _context.SaveChanges();

        }
    }


}
