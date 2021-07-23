using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IInvitationData
    {
        Invitation Add(Invitation newInvitation);
        Invitation Delete(string userId, int guildId);
        IEnumerable<Invitation> GetByUserId(string userId);
        IEnumerable<Invitation> GetByGuildId(int guildId);
        Invitation Find(string userId, int guildId);
        int Commit();
    }
}
