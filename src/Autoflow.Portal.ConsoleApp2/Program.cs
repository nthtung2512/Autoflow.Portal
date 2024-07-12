using Autoflow.Portal.Auth.Configurations;
using Autoflow.Portal.ConsoleApp;
using Autoflow.Portal.ConsoleApp.Services;
using Autoflow.Portal.Domain.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Client;

//Build the configuration
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Read the client info from configuration
var clientInfo = config.GetSection("ClientConfiguration:AuthenticationEndpoint").Get<AuthenticationEndPoint>();


var services = new ServiceCollection();


services.AddOpenIddict()

    // Register the OpenIddict client components.
    .AddClient(options =>
    {
        // Allow grant_type=client_credentials to be negotiated.
        options.AllowClientCredentialsFlow();

        // Disable token storage, which is not necessary for non-interactive flows like
        // grant_type=password, grant_type=client_credentials or grant_type=refresh_token.
        options.DisableTokenStorage();

        // Register the System.Net.Http integration and use the identity of the current
        // assembly as a more specific user agent, which can be useful when dealing with
        // providers that use the user agent as a way to throttle requests (e.g., Reddit).
        options.UseSystemNetHttp()
               .SetProductInformation(typeof(Program).Assembly);

        // Add a client registration matching the client application definition in the server project.
        options.AddRegistration(new OpenIddictClientRegistration
        {
            Issuer = new Uri("https://localhost:7198/", UriKind.Absolute),
            ClientId = clientInfo.ClientId,
            ClientSecret = clientInfo.ClientSecret
        });
    });

await using var provider = services.BuildServiceProvider();


var token = await provider.GetTokenAsync();

Console.WriteLine("Access token: {0}", token);
Console.WriteLine();

var fullClientInfo = await provider.GetClientAsync<ClientInfo>(token);
var organizationName = string.Empty;
HubConnection hubConnection = null;
var organizationMessages = new List<string>();

if (fullClientInfo.Role == "User")
{
    organizationName = await provider.GetOrganizationNameAsync(token);
    var hubUrl = "https://localhost:7198/chatHub";
    hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();

    // Set up listeners for SignalR messages
    hubConnection.On<string>("ReceiveSimpleMessage", (message) =>
    {
        //organizationMessages.Add(message);
        Console.WriteLine(message);
    });

    await hubConnection.StartAsync();

    // Join a group with the organizationId
    await hubConnection.InvokeAsync("JoinGroup", fullClientInfo.OrganizationId);
}

while (true)
{
    Console.WriteLine("Choose an option:");
    Console.WriteLine("1. Send message");
    Console.WriteLine("2. Add organization");
    Console.WriteLine("q. Quit");

    var choice = Console.ReadLine();
    if (choice.Equals("q", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Exiting the application.");
        Environment.Exit(0);
    }

    switch (choice)
    {
        case "1":
            if (fullClientInfo.Role == "Admin")
            {
                Console.WriteLine("Access to this feature is not allowed for Admins.");
                Console.WriteLine();
                break;
            }
            // Initialize the messages list from the server
            organizationMessages = await provider.GetMessageAsync(token);
            provider.PrintOrganizationMessages(organizationMessages, fullClientInfo, organizationName);
            while (true)
            {
                await provider.SendMessagesOptionAsync(organizationMessages, fullClientInfo, organizationName, token, hubConnection);
            }

        case "2":
            if (fullClientInfo.Role == "User")
            {
                Console.WriteLine("Access to this feature is not allowed for User.");
                Console.WriteLine();
                break;
            }
            await provider.AddOrganizationOption(token, fullClientInfo);
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}

