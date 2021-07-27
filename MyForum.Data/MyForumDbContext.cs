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
        public DbSet<Like> Likes { get; set; }

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

                entity.HasOne(m => m.Guild)
                    .WithMany(m => m.ChatMessages)
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
                entity.HasOne(g => g.Guildmaster)
                    .WithOne(g => g.ManagedGuild)
                    .HasForeignKey<Guild>(g => g.GuildmasterId);

                entity.HasIndex(g => g.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<MyUser>(entity =>
            {
                entity.HasMany(u => u.GuildsMembership)
                    .WithMany(u => u.Members);
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasOne(l => l.FromUser)
                    .WithMany(l => l.GivenLikes)
                    .HasForeignKey(l => l.FromUserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(l => l.Message)
                    .WithMany(l => l.Likes)
                    .HasForeignKey(l => l.MessageId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasKey(l => new { l.FromUserId, l.MessageId });
            });
        }
    }
}
