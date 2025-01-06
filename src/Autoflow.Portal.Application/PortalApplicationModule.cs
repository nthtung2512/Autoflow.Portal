using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Base;
using Autoflow.Portal.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Autoflow.Portal.Application
{
    public class PortalApplicationModule : Module
    {
        public override void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IUserConversationMapRepository, UserConversationMapRepository>();
        }
    }
}
