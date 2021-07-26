using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IInvitationRepository
    {
        void Add(Invitation newInvitation);
        void Delete(string userId, int guildId);
        Invitation Get(string userId, int guildId);
        IEnumerable<Invitation> GetByUserId(string userId);
        bool IsUserHavingAnyInvitation(string userId);
    }
}
