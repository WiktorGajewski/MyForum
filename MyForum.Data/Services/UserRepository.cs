using Microsoft.EntityFrameworkCore;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyForum.Data.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly MyForumDbContext _context;

        public UserRepository(MyForumDbContext context)
        {
            this._context = context;
        }

        public void Delete(string id)
        {
            var user = GetById(id);

            if(user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public int CountUsers(string nameFilter = null)
        {
            return _context.Users
                .Where(u => nameFilter == null || u.UserName.StartsWith(nameFilter))
                .Count();
        }

        public MyUser GetById(string id)
        {
            return _context.Users.Find(id);
        }

        public MyUser GetByIdWithMembershipData(string id)
        {
            return _context.Users
                .Include(u => u.GuildsMembership)
                .FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<MyUser> GetByUserName(string userNameFilter, int usersToTake, int usersToSkip)
        {
            return _context.Users
                .Where(u => userNameFilter == null || u.UserName.StartsWith(userNameFilter))
                .OrderByDescending(u => u.Rank)
                .ThenByDescending(u => u.PrestigePoints)
                .Skip(usersToSkip)
                .Take(usersToTake)
                .ToList();
        }

        public void ChangeRank(string id, Rank rank)
        {
            var user = GetById(id);

            if(user != null)
            {
                user.Rank = rank;
                _context.SaveChanges();
            }
        }

        public Guild GetManagedGuild(string userId)
        {
            var user = _context.Users
                .Include(u => u.ManagedGuild)
                .FirstOrDefault(u => u.Id == userId);

            return user?.ManagedGuild;
        }

        public IEnumerable<MyUser> GetGuildmastersWithoutGuild()
        {
            return _context.Users
                .Where(u => u.Rank == Rank.Guildmaster && u.ManagedGuildId == null)
                .ToList();
        }
    }
}
