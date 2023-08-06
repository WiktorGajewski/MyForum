using System.Collections.Generic;
using MyForum.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyForum.Data
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
            var query = from b in db.Guilds
                        where string.IsNullOrEmpty(name) || b.Name.StartsWith(name)
                        orderby b.Name
                        select b;
            return query;
        }

        public Guild Update(Guild updatedGuild)
        {
            var entity = db.Guilds.Attach(updatedGuild);
            entity.State = EntityState.Modified;
            return updatedGuild;
        }
    }
}
