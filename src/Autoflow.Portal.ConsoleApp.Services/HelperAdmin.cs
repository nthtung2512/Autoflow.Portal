using Autoflow.Portal.Domain.Organization;
using Autoflow.Portal.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;

namespace Autoflow.Portal.ConsoleApp
{
    public static class HelperAdmin
    {
        public static async Task AddOrganizationAsync(this IServiceProvider provider, string token, Organization newOrganization)
        {
            using var client = provider.GetRequiredService<HttpClient>();

            string endpoint = "https://localhost:7198/api/organization";

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException("Access to organization messages is forbidden.");
            }
            else
            {
                // Handle other status codes if necessary
                response.EnsureSuccessStatusCode(); // This will throw an exception for other status codes
            }
        }

        public static async Task AddOrganizationOption(this IServiceProvider provider, string token, ClientInfo fullClientInfo)
        {
            if (fullClientInfo.Role == "User")
            {
                Console.WriteLine("You don't have access to this feautre."); return;
            }
            Console.Clear();
            while (true)
            {
                Console.Write("Enter the organization name (Press q or Q to quit): ");
                var organizationName = Console.ReadLine();
                if (organizationName.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting the application.");
                    Environment.Exit(0);
                }
                var newOrganization = new Organization(new Guid()) { OrganizationName = organizationName, OrganizationMessages = [] };
                await provider.AddOrganizationAsync(token, newOrganization);
                Console.WriteLine("Organization added successfully.");
            }
        }

    }
}
