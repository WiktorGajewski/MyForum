using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyForum.Core;
using MyForum.Data;

namespace MyForum.Pages.B2
{
    public class CreateModel : PageModel
    {
        private readonly MyForum.Data.MyForumDbContext _context;

        public CreateModel(MyForum.Data.MyForumDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Banner Banner { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Banners.Add(Banner);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
