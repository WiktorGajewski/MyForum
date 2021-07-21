using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IUserData
    {
        IEnumerable<MyUser> GetByUsername(string nickname);
        IEnumerable<MyUser> GetByUsername(string username, int usersToTake, int usersToSkip);
        int CountUsers();
        MyUser GetById(string id);
        MyUser Delete(string id);
        int Commit();
        MyUser SetUpNewRank(string id, Rank rank);
    }
}
