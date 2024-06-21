using Autoflow.Portal.Domain.ChatBox;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore
{
    public class PortalDbContext(DbContextOptions<PortalDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserConversationMap> UserConversationMaps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuring User
            builder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasKey(u => u.Id);

                b.HasMany(u => u.SentMessages)
                 .WithOne(m => m.Sender)
                 .HasForeignKey(m => m.SendUserId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(u => u.ReceivedMessages)
                 .WithOne(m => m.Receiver)
                 .HasForeignKey(m => m.ReceiveUserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuring Message
            builder.Entity<Message>(b =>
            {
                b.ToTable("Messages");
                b.HasKey(m => m.Id);

                b.HasOne(m => m.Conversation)
                 .WithMany(c => c.Messages)
                 .HasForeignKey(m => m.ConversationId);

                b.HasOne(m => m.Sender)
                 .WithMany(u => u.SentMessages)
                 .HasForeignKey(m => m.SendUserId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(m => m.Receiver)
                 .WithMany(u => u.ReceivedMessages)
                 .HasForeignKey(m => m.ReceiveUserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuring Conversation
            builder.Entity<Conversation>(b =>
            {
                b.ToTable("Conversations");
                b.HasKey(c => c.Id);

                b.HasMany(c => c.Messages)
                 .WithOne(m => m.Conversation)
                 .HasForeignKey(m => m.ConversationId);
            });

            // Configuring UserConversationMap
            // How to create multi-columns key
            builder.Entity<UserConversationMap>(b =>
            {
                b.ToTable("UserConversationMaps");
                b.HasKey(uc => new { uc.UserId, uc.ConversationId });

                b.HasOne(uc => uc.User)
                 .WithMany(u => u.UserConversations)
                 .HasForeignKey(uc => uc.UserId);

                b.HasOne(uc => uc.Conversation)
                 .WithMany(c => c.UserConversations)
                 .HasForeignKey(uc => uc.ConversationId);
            });
        }
    }
}
