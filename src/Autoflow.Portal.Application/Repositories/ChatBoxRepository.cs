using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;

// Implement the methods for CRUP API for User, Message, and Conversation
namespace Autoflow.Portal.Infrastructure.Repositories
{
    public abstract class ChatBoxRepository : IDisposable
    {
        protected readonly PortalDbContext _context;

        protected ChatBoxRepository(PortalDbContext context)
        {
            _context = context;
        }

        // Additional methods as needed for your application
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
