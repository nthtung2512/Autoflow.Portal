using Autoflow.Portal.Domain.ChatBox;
using Autoflow.Portal.Domain.Shared.Roles;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Portal.Application.Repositories
{
    public class PermissionRepository : IDisposable
    {
        private readonly PortalDbContext _context;
        private DbSet<ApplicationPermission> _roles => _context.Set<ApplicationPermission>();

        public PermissionRepository(PortalDbContext context)
        {
            _context = context;
        }

        public async Task<UserRole[]> SelectAllUserRolesAsync(Guid userId)
        {
            return await _roles
                .Where(role => role.User.Id == userId)
                .Select(role => role.Role)
                .ToArrayAsync();
        }

        #region dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    _context.Dispose();
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
