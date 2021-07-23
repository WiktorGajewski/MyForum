using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyForum.Core;

namespace MyForum.Data
{
    public class MyForumDbContext : IdentityDbContext<MyUser>
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        public MyForumDbContext(DbContextOptions<MyForumDbContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasOne(m => m.FromUser)
                    .WithMany(m => m.ChatMessages)
                    .HasForeignKey(m => m.FromUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(m => m.GuildForum)
                    .WithMany(m => m.ForumChatMessages)
                    .HasForeignKey(m => m.GuildId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Invitation>(entity =>
            {
                entity.HasOne(i => i.User)
                    .WithMany(i => i.Invitations)
                    .HasForeignKey(i => i.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(i => i.Guild)
                    .WithMany(i => i.Invitations)
                    .HasForeignKey(i => i.GuildId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasKey(i => new { i.GuildId, i.UserId });
            });

            modelBuilder.Entity<Guild>(entity =>
            {
                entity.HasOne(u => u.Guildmaster)
                    .WithOne(u => u.ManagedGuild)
                    .HasForeignKey<MyUser>(g => g.ManagedGuildId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<MyUser>(entity =>
            {
                entity.HasMany(u => u.GuildsMembership)
                    .WithMany(u => u.Members);
            });
        }
    }
}
