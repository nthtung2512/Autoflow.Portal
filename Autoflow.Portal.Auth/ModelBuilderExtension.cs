using Autoflow.Portal.Domain.ChatBox;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Portal.Auth
{
    public static class ModelBuilderExtension
    {
        public static void UseAuthPrivateEntities(this ModelBuilder builder)
        {
            builder.Entity<ApplicationPermission>(b =>
            {
                b.ToTable(
                    "" + "permission" + "auth"
                );
            });
        }
    }
}
