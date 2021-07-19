using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IUserData
    {
        IEnumerable<MyUser> GetByUsername(string nickname);
        MyUser GetById(string id);
        MyUser Update(MyUser updatedUser);
        MyUser Add(MyUser newUser);
        MyUser Delete(string id);
        int Commit();
    }
}
