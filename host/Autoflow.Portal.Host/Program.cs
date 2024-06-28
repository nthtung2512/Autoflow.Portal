using Autoflow.Portal.Base;
using Autoflow.Portal.Host;
using Autoflow.Portal.Host.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;

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


//Set Up Authentication for cookies and JWT tokens
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    //options.Events = new JwtBearerEvents
    //{
    //    OnMessageReceived = context =>
    //    {
    //        var accessToken = context.Request.Query["access_token"];
    //        Console.WriteLine("Access token", accessToken);
    //        // If the request is for our hub...
    //        var path = context.HttpContext.Request.Path;
    //        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chatHub")))
    //        {
    //            context.Token = accessToken;
    //        }
    //        return Task.CompletedTask;
    //    }
    //};
});

builder.Services.AddAuthorization();


builder.Services.AddCors((options) =>
{
    options.AddPolicy("PortalChatBox",
        new CorsPolicyBuilder()
            .WithOrigins("http://localhost:5173")
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
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("PortalChatBox");
app.MapHub<ChatHub>("/chatHub");
// Maps controller endpoints to the request pipeline, setting up routes to handle incoming HTTP requests.
app.MapControllers();

// Starts the application and begins listening for incoming HTTP requests.
app.Run();
