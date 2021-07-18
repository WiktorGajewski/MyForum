using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IGuildData
    {
        IEnumerable<Guild> GetByName(string name);
        Guild GetById(int id);
        Guild Update(Guild updatedGuild);
        Guild Add(Guild newGuild);
        Guild Delete(int id);
        int Commit();
    }
}
