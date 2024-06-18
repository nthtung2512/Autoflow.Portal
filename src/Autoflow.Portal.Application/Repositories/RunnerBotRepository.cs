using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;

namespace Autoflow.Portal.Application.Repositories
{
    public class RunnerBotRepository : IRunnerBotRepository, IDisposable
    {
        private readonly PortalDbContext _context;

        public RunnerBotRepository(PortalDbContext context)
        {
            _context = context;
        }

        #region dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) _context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
