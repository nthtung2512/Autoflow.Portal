using Autoflow.Portal.Base.EFCore;

namespace Autoflow.Portal.Domain.RunnerBots
{
    public class RunnerEvent : Entity<int>
    {
        public bool IsSuccess { get; set; } = false;
        public required string Message { get; set; }
        public required RunnerBot Bot { get; set; }
    }
}
