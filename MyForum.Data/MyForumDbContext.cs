using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyForum.Core;

namespace MyForum.Data
{
    public class MyForumDbContext : IdentityDbContext<MyUser>
    {
        public DbSet<Guild> Guilds { get; set; }

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
