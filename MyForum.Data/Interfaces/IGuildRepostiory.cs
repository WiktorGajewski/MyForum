using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IGuildRepostiory
    {
        void Add(Guild newGuild);
        void Update(Guild updatedGuild);
        void Delete(int id);
        int CountGuilds(string nameFilter = null);
        Guild GetById(int id);
        Guild GetByIdWithMembersData(int id);
        IEnumerable<Guild> GetByName(string nameFilter, int guildsToTake, int guildsToSkip);
        void AssignGuildmaster(int guildId, string guildmasterId);
        void AddMember(int guildId, string memberId);
        void RemoveMember(int guildId, string memberId);
        bool CheckNameUnique(string guildNameCheck);
    }
}
