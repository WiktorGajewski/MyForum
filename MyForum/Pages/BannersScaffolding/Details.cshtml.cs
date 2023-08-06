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
    public class DetailsModel : PageModel
    {
        private readonly MyForum.Data.MyForumDbContext _context;

        public DetailsModel(MyForum.Data.MyForumDbContext context)
        {
            _context = context;
        }

        public Banner Banner { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Banner = await _context.Banners.FirstOrDefaultAsync(m => m.Id == id);

            if (Banner == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
