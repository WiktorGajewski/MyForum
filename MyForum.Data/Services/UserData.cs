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

        public User Add(User newUser)
        {
            db.Users.Add(newUser);
            return newUser;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public User Delete(int id)
        {
            var user = GetById(id);
            if(user != null)
            {
                db.Users.Remove(user);
            }
            return user;
        }

        public User GetById(int id)
        {
            return db.Users.Find(id);
        }

        public IEnumerable<User> GetByNickname(string nickname)
        {
            var query = from u in db.Users
                        where nickname == null || u.Nickname.StartsWith(nickname)
                        orderby u.PrestigePoints
                        select u;
            return query;
        }

        public User Update(User updatedUser)
        {
            var entity = db.Users.Attach(updatedUser);
            entity.State = EntityState.Modified;
            return updatedUser;
        }
    }
}
