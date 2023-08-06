using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data
{
    public interface IBannerData
    {
        IEnumerable<Banner> GetByName(string name);
        Banner GetById(int id);
        Banner Update(Banner updatedBanner);
        Banner Add(Banner newBanner);
        Banner Delete(int id);
        int Commit();
    }
}
