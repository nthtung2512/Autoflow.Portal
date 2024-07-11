using Autoflow.Portal.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Client;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Autoflow.Portal.ConsoleApp
{
    public static class Helper
    {
        public static async Task<string> GetTokenAsync(this IServiceProvider provider)
        {
            var service = provider.GetRequiredService<OpenIddictClientService>();

            var result = await service.AuthenticateWithClientCredentialsAsync(new());
            return result.AccessToken;
        }
        public static async Task<List<string>> GetMessageAsync(this IServiceProvider provider, string token)
        {
            using var client = provider.GetRequiredService<HttpClient>();

            string endpoint = "https://localhost:7198/api/organization/messages";

            using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<string>>(responseBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException("Access to organization messages is forbidden.");
            }
            else
            {
                // Handle other status codes if necessary
                response.EnsureSuccessStatusCode(); // This will throw an exception for other status codes
                return null;
            }
        }
        public static async Task<string> GetOrganizationNameAsync(this IServiceProvider provider, string token)
        {
            using var client = provider.GetRequiredService<HttpClient>();

            string endpoint = "https://localhost:7198/api/organization/user";

            using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return "Access to organization name is forbidden.";
            }
            else
            {
                // Handle other status codes if necessary
                response.EnsureSuccessStatusCode(); // This will throw an exception for other status codes
                return null;
            }
        }
        public static async Task<T> GetClientAsync<T>(this IServiceProvider provider, string token)
        {
            using var client = provider.GetRequiredService<HttpClient>();

            string endpoint = "https://localhost:7198/api/client";

            using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException("Access to organization messages is forbidden.");
            }
            else
            {
                // Handle other status codes if necessary
                response.EnsureSuccessStatusCode(); // This will throw an exception for other status codes
                return default;
            }
        }
        public static async Task SendMessageAsync(this IServiceProvider provider, string token, string message)
        {
            using var client = provider.GetRequiredService<HttpClient>();

            string endpoint = "https://localhost:7198/api/organization/messages";

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(JsonSerializer.Serialize(message), System.Text.Encoding.UTF8, "application/json");
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
        public static void PrintOrganizationMessages(this IServiceProvider provider, List<string> messages, ClientInfo fullClientInfo, string organizationName)
        {
            Console.Clear();
            Console.WriteLine($"Welcome Client {fullClientInfo.ClientId} from {organizationName} to the app.");
            Console.WriteLine("Organization messages:");
            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
            Console.WriteLine();
        }
    }
}
