using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IUserData
    {
        IEnumerable<User> GetByNickname(string nickname);
        User GetById(int id);
        User Update(User updatedUser);
        User Add(User newUser);
        User Delete(int id);
        int Commit();
    }
}
