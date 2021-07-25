using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;

namespace MyForum.Pages.Guilds
{
    [Authorize(Policy = "IsLeader")]
    public class DeleteModel : PageModel
    {
        private readonly IGuildRepostiory guildData;

        public Guild Guild { get; set; }

        public DeleteModel(IGuildRepostiory guildData)
        {
            this.guildData = guildData;
        }

        public IActionResult OnGet(int guildId)
        {
            Guild = guildData.GetById(guildId);

            if(Guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            return Page();
        }

        public IActionResult OnPost(int guildId)
        {
            guildData.Delete(guildId);

            TempData["Message"] = $"Guild deleted";
            return RedirectToPage("./Index");
        }
    }
}
