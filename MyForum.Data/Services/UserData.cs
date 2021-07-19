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

        public MyUser Add(MyUser newUser)
        {
            db.Users.Add(newUser);
            return newUser;
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

        public IEnumerable<MyUser> GetByUsername(string username)
        {
            var query = from u in db.Users
                        where username == null || u.UserName.StartsWith(username)
                        orderby u.PrestigePoints
                        select u;
            return query;
        }

        public MyUser Update(MyUser updatedUser)
        {
            var entity = db.Users.Attach(updatedUser);
            entity.State = EntityState.Modified;
            return updatedUser;
        }
    }
}
