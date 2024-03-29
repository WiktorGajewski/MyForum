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
            Guild = guildData.GetById(guildId);

            if(Guild == null)
            {
                return RedirectToPage("./NotFound");
            }

            return Page();
        }
    }
}
