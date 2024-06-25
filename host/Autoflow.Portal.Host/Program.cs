using Autoflow.Portal.Base;
using Autoflow.Portal.Host;
using Autoflow.Portal.Host.Hubs;
using Microsoft.AspNetCore.Cors.Infrastructure;
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
// Create a builder for the web application
var builder = WebApplication.CreateBuilder(args);

// Configure Serilog as the logging provider
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddSignalR();

// Register services with the dependency injection (DI) container
builder.Services.AddControllers();          // Add services for controllers to the container
builder.Services.AddEndpointsApiExplorer(); // Add services for API endpoint exploration (useful for tools like Swagger)
builder.Services.AddSwaggerGen();           // Add services required to generate Swagger documentation

// Register custom modules with the DI container
builder.AddModules<AutoflowPortalApiModule>();

builder.Services.AddCors((options) =>
{
    options.AddPolicy("PortalChatBox",
        new CorsPolicyBuilder()
            .WithOrigins("http://localhost:5173")
            .WithOrigins("http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .Build());
});



// Build the web application
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment()) // Check if the app is running in the Development environment
{
    app.UseSwagger();    // Enable middleware to serve generated Swagger as a JSON endpoint
    app.UseSwaggerUI();  // Enable middleware to serve the Swagger UI
}

// Enable middleware to redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Adds middleware to handle authorization checks for incoming requests.
app.UseAuthorization();

app.UseCors("PortalChatBox");
app.MapHub<ChatHub>("/chatHub");

// Maps controller endpoints to the request pipeline, setting up routes to handle incoming HTTP requests.
app.MapControllers();

// Starts the application and begins listening for incoming HTTP requests.
app.Run();
