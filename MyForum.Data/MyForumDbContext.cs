using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyForum.Core;

namespace MyForum.Data
{
    public class MyForumDbContext : IdentityDbContext<MyUser>
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }

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
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(m => m.GuildForum)
                    .WithMany(m => m.ForumChatMessages)
                    .HasForeignKey(m => m.GuildId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
