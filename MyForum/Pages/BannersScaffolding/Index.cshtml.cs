using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyForum.Core;
using MyForum.Data;

namespace MyForum.Pages.B2
{
    public class IndexModel : PageModel
    {
        private readonly MyForum.Data.MyForumDbContext _context;

        public IndexModel(MyForum.Data.MyForumDbContext context)
        {
            _context = context;
        }

        public IList<Banner> Banner { get;set; }

        public async Task OnGetAsync()
        {
            Banner = await _context.Banners.ToListAsync();
        }
    }
}
