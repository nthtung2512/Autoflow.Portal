using Autoflow.Portal.Base.EFCore;

namespace Autoflow.Portal.Domain.RunnerBots
{
    public class RunnerBot : Entity<Guid>
    {
        public required string Name { get; set; }
        public required BotStatus Status { get; set; }
    }
}
