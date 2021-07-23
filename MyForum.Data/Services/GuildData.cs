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
        private readonly IUserData userData;

        public GuildData(MyForumDbContext db, IUserData userData)
        {
            this.db = db;
            this.userData = userData;
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

        public Guild GetByIdWithMembers(int id)
        {
            return db.Guilds.Where(g => g.Id == id)
                .Include(g => g.Members)
                .FirstOrDefault();
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

        public Guild AssignGuildmaster(int guildId, string guildmasterId)
        {
            var guild = GetById(guildId);
            var user = userData.GetById(guildmasterId);

            if (guild != null && user != null)
            {
                guild.GuildmasterId = guildmasterId;
                guild.Guildmaster = user;
                user.ManagedGuildId = guildId;
                user.ManagedGuild = guild;
            }

            return guild;
        }

        public Guild AddMember(int guildId, string memberId)
        {
            var guild = GetByIdWithMembers(guildId);
            var user = userData.GetByIdWithGuilds(memberId);

            if(guild != null && user != null)
            {
                guild.Members.Add(user);
                user.GuildsMembership.Add(guild);
            }

            return guild;
        }

        public Guild RemoveMember(int guildId, string memberId)
        {
            var guild = GetById(guildId);
            var user = userData.GetById(memberId);

            if (guild != null && user != null)
            {
                guild.Members.Remove(user);
                user.GuildsMembership.Remove(guild);
            }

            return guild;
        }
    }
}
