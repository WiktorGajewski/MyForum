using Microsoft.EntityFrameworkCore;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyForum.Data.Services
{
    public class UserData : IUserData
    {
        private readonly MyForumDbContext db;

        public UserData(MyForumDbContext db)
        {
            this.db = db;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public MyUser Delete(string id)
        {
            var user = GetById(id);
            if(user != null)
            {
                db.Users.Remove(user);
            }
            return user;
        }

        public MyUser GetById(string id)
        {
            return db.Users.Find(id);
        }

        public MyUser GetByIdWithGuilds(string id)
        {
            return db.Users.Where(u => u.Id == id)
                .Include(u => u.GuildsMembership)
                .FirstOrDefault();
        }

        public IEnumerable<MyUser> GetByUsername(string username)
        {
            var query = db.Users
                .Where(u => username == null || u.UserName.StartsWith(username))
                .OrderByDescending(u => u.Rank)
                .ThenByDescending(u => u.PrestigePoints)
                .ToList();

            return query;
        }

        public IEnumerable<MyUser> GetByUsername(string username, int usersToTake, int usersToSkip)
        {
            var query = db.Users
                .Where(u => username == null || u.UserName.StartsWith(username))
                .OrderByDescending(u => u.Rank)
                .ThenByDescending(u => u.PrestigePoints)
                .Skip(usersToSkip)
                .Take(usersToTake)
                .ToList();

            return query;
        }

        public int CountUsers(string name = null)
        {
            return db.Users
                .Where(u => name == null || u.UserName.StartsWith(name))
                .Count();
        }

        public MyUser SetUpNewRank(string id, Rank rank)
        {
            var user = GetById(id);
            user.Rank = rank;
            return user;
        }

        public int? GetManagedGuildId(string userId)
        {
            var user = GetById(userId);

            if(user != null)
            {
                return user.ManagedGuildId;
            }

            return null;
        }
    }
}
