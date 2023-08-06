using Microsoft.EntityFrameworkCore;
using MyForum.Core;

namespace MyForum.Data
{
    public class MyForumDbContext : DbContext
    {
        public MyForumDbContext(DbContextOptions<MyForumDbContext> options)
            :base(options)
        {

        }

        public DbSet<Banner> Banners { get; set; }
    }
}
