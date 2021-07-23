using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IGuildData
    {
        IEnumerable<Guild> GetByName(string name);
        IEnumerable<Guild> GetByName(string name, int guildsToTake, int guildsToSkip);
        int CountGuilds(string name = null);
        Guild GetById(int id);
        Guild GetByIdWithMembers(int id);
        Guild Update(Guild updatedGuild);
        Guild Add(Guild newGuild);
        Guild Delete(int id);
        int Commit();
        Guild AssignGuildmaster(int guildId, string guildmasterId);
        Guild AddMember(int guildId, string memberId);
        Guild RemoveMember(int guildId, string memberId);
    }
}
