using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IUserData
    {
        IEnumerable<MyUser> GetByUsername(string nickname);
        MyUser GetById(string id);
        MyUser Delete(string id);
        int Commit();
    }
}
