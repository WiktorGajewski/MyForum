using System.Collections.Generic;
using MyForum.Core;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace MyForum.Data
{
    public class SqlBannerData : IBannerData
    {
        private readonly MyForumDbContext db;

        public SqlBannerData(MyForumDbContext db)
        {
            this.db = db;
        }

        public Banner Add(Banner newBanner)
        {
            db.Add(newBanner);
            return newBanner;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Banner Delete(int id)
        {
            var banner = GetById(id);
            if(banner != null)
            {
                db.Banners.Remove(banner);
            }
            return banner;
        }

        public Banner GetById(int id)
        {
            return db.Banners.Find(id);
        }

        public IEnumerable<Banner> GetByName(string name)
        {
            var query = from b in db.Banners
                        where string.IsNullOrEmpty(name) || b.Name.StartsWith(name)
                        orderby b.Name
                        select b;
            return query;
        }

        public Banner Update(Banner updatedBanner)
        {
            var entity = db.Banners.Attach(updatedBanner);
            entity.State = EntityState.Modified;
            return updatedBanner;
        }
    }
}
