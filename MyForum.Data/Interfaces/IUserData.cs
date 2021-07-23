using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IUserData
    {
        IEnumerable<MyUser> GetByUsername(string username);
        IEnumerable<MyUser> GetByUsername(string username, int usersToTake, int usersToSkip);
        int CountUsers(string name = null);
        MyUser GetById(string id);
        MyUser GetByIdWithGuilds(string id);
        MyUser Delete(string id);
        int Commit();
        MyUser SetUpNewRank(string id, Rank rank);
        int? GetManagedGuildId(string userId);
    }
}
