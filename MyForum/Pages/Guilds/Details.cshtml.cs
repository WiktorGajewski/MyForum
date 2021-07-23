using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;

namespace MyForum.Pages.Guilds
{
    public class DetailsModel : PageModel
    {
        private readonly IGuildData guildData;

        [TempData]
        public string Message { get; set; }

        public Guild Guild { get; set; }

        public DetailsModel(IGuildData guildData)
        {
            this.guildData = guildData;
        }

        public IActionResult OnGet(int guildId)
        {
            Guild = guildData.GetByIdWithMembers(guildId);

            if(Guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            return Page();
        }
    }
}
