using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IUserRepository
    {
        void Delete(string id);
        int CountUsers(string nameFilter = null);
        MyUser GetById(string id);
        MyUser GetByIdWithMembershipData(string id);
        IEnumerable<MyUser> GetByUserName(string userNameFilter, int usersToTake, int usersToSkip);
        void ChangeRank(string id, Rank rank);
        Guild GetManagedGuild(string userId);
        IEnumerable<MyUser> GetGuildmastersWithoutGuild();
    }
}
