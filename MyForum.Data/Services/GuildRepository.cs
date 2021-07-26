using System.Collections.Generic;
using MyForum.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyForum.Data.Interfaces;

namespace MyForum.Data.Services
{
    public class GuildRepository : IGuildRepostiory
    {
        private readonly MyForumDbContext _context;
        private readonly IUserRepository _userData;

        public GuildRepository(MyForumDbContext context, IUserRepository userData)
        {
            this._context = context;
            this._userData = userData;
        }

        public void Add(Guild newGuild)
        {
            _context.Add(newGuild);
            _context.SaveChanges();
        }

        public void Update(Guild updatedGuild)
        {
            _context.Guilds.Update(updatedGuild);
            _context.SaveChanges();
        }

        public bool CheckNameUnique(string guildNameCheck)
        {
            return _context.Guilds
                .Any(g => g.Name == guildNameCheck);
        }

        public void Delete(int id)
        {
            var guild = GetById(id);

            if(guild != null)
            {
                _context.Guilds.Remove(guild);
                _context.SaveChanges();
            }
        }

        public int CountGuilds(string nameFilter = null)
        {
            return _context.Guilds
                .Where(g => nameFilter == null || g.Name.StartsWith(nameFilter))
                .Count();
        }

        public Guild GetById(int id)
        {
            return _context.Guilds.Find(id);
        }

        public Guild GetByIdWithMembersData(int id)
        {
            return _context.Guilds
                .Include(g => g.Members)
                .FirstOrDefault(g => g.Id == id);
        }

        public IEnumerable<Guild> GetByName(string nameFilter, int guildsToTake, int guildsToSkip)
        {
            return _context.Guilds
                .Where(g => nameFilter == null || g.Name.StartsWith(nameFilter))
                .OrderBy(g => g.Name)
                .Skip(guildsToSkip)
                .Take(guildsToTake)
                .ToList();
        }

        public void AssignGuildmaster(int guildId, string guildmasterId)
        {
            var guild = GetById(guildId);
            var user = _userData.GetById(guildmasterId);

            if (guild != null && user != null)
            {
                guild.GuildmasterId = guildmasterId;
                guild.Guildmaster = user;
                user.ManagedGuild = guild;
                user.ManagedGuildId = guildId;
                _context.SaveChanges();
            }
        }

        public void RemoveGuildmaster(int guildId, string guildmasterId)
        {
            var guild = GetById(guildId);
            var user = _userData.GetById(guildmasterId);

            if (guild != null && user != null)
            {
                guild.GuildmasterId = null;
                guild.Guildmaster = null;
                user.ManagedGuild = null;
                user.ManagedGuildId = null;
                _context.SaveChanges();
            }
        }

        public void AddMember(int guildId, string memberId)
        {
            var guild = GetByIdWithMembersData(guildId);
            var user = _userData.GetByIdWithMembershipData(memberId);

            if(guild != null && user != null)
            {
                guild.Members.Add(user);
                user.GuildsMembership.Add(guild);
                _context.SaveChanges();
            }
        }

        public void RemoveMember(int guildId, string memberId)
        {
            var guild = GetByIdWithMembersData(guildId);
            var user = _userData.GetByIdWithMembershipData(memberId);

            if (guild != null && user != null)
            {
                guild.Members.Remove(user);
                user.GuildsMembership.Remove(guild);
                _context.SaveChanges();
            }
        }
    }
}
