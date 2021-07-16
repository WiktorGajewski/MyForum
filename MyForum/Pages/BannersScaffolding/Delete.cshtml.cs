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
    public class DeleteModel : PageModel
    {
        private readonly MyForum.Data.MyForumDbContext _context;

        public DeleteModel(MyForum.Data.MyForumDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Banner = await _context.Banners.FindAsync(id);

            if (Banner != null)
            {
                _context.Banners.Remove(Banner);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
