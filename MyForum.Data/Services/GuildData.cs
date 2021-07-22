using System.Collections.Generic;
using MyForum.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyForum.Data.Interfaces;

namespace MyForum.Data.Services
{
    public class GuildData : IGuildData
    {
        private readonly MyForumDbContext db;

        public GuildData(MyForumDbContext db)
        {
            this.db = db;
        }

        public Guild Add(Guild newGuild)
        {
            db.Add(newGuild);
            return newGuild;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Guild Delete(int id)
        {
            var guild = GetById(id);
            if(guild != null)
            {
                db.Guilds.Remove(guild);
            }
            return guild;
        }

        public Guild GetById(int id)
        {
            return db.Guilds.Find(id);
        }

        public IEnumerable<Guild> GetByName(string name)
        {
            var query = db.Guilds
                .Where(g => name == null || g.Name.StartsWith(name))
                .OrderBy(g => g.Name)
                .ToList();

            return query;
        }

        public IEnumerable<Guild> GetByName(string name, int guildsToTake, int guildsToSkip)
        {
            var query = db.Guilds
                .Where(g => name == null || g.Name.StartsWith(name))
                .OrderBy(g => g.Name)
                .Skip(guildsToSkip)
                .Take(guildsToTake)
                .ToList();

            return query;
        }

        public int CountGuilds(string name = null)
        {
            return db.Guilds
                .Where(g => name == null || g.Name.StartsWith(name))
                .Count();
        }

        public Guild Update(Guild updatedGuild)
        {
            var entity = db.Guilds.Attach(updatedGuild);
            entity.State = EntityState.Modified;
            return updatedGuild;
        }
    }
}
