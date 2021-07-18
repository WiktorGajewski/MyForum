using Microsoft.EntityFrameworkCore;
using MyForum.Core;

namespace MyForum.Data
{
    public class MyForumDbContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<User> Users { get; set; }

        public MyForumDbContext(DbContextOptions<MyForumDbContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
