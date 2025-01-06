using Autoflow.Portal.Base;
using Autoflow.Portal.Host;
using Autoflow.Portal.Host.Hubs;
using Serilog;
using Serilog.Events;

#region LoggerConfiguration
Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs.txt"))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();
#endregion

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!.ToString();
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true)
    .Build();
// Create a builder for the web application
var builder = WebApplication.CreateBuilder(args);


// Configure Serilog as the logging provider
builder.Host.UseSerilog();

// Register custom modules with the DI container
builder.Services.AddModules<AutoflowPortalApiModule>();

// Build the web application
var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseRouting();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment()) // Check if the app is running in the Development environment
{
    app.UseSwagger();    // Enable middleware to serve generated Swagger as a JSON endpoint
    app.UseSwaggerUI();  // Enable middleware to serve the Swagger UI
}

// Enable middleware to redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Adds middleware to handle authorization checks for incoming requests.
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("PortalChatBox");
app.MapHub<ChatHub>("/chatHub");
// Maps controller endpoints to the request pipeline, setting up routes to handle incoming HTTP requests.
app.MapControllers();
app.MapDefaultControllerRoute();

// Starts the application and begins listening for incoming HTTP requests.
app.Run();
