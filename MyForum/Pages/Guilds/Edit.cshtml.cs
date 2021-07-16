using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data;

namespace MyForum.Pages.Guilds
{
    public class EditModel : PageModel
    {
        private readonly IGuildData guildData;

        [BindProperty]
        public Guild Guild { get; set; }

        [ViewData]
        public string Title { get; set; }

        public EditModel(IGuildData guildData)
        {
            this.guildData = guildData;
        }

        public IActionResult OnGet(int? guildId)
        {
            if(guildId.HasValue)
            {
                Guild = guildData.GetById(guildId.Value);
                Title = "Edit";
            }
            else
            {
                Guild = new Guild();
                Title = "Create";
            }
            
            if(Guild == null)
            {
                return RedirectToPage("./NotFound");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            if(Guild.Id > 0)
            {
                Guild = guildData.Update(Guild);
            }
            else
            {
                Guild = guildData.Add(Guild);
            }
            
            guildData.Commit();
            TempData["Message"] = "Guild saved";
            return RedirectToPage("./Details", new { guildId = Guild.Id });
        }
    }
}
