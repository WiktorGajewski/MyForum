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
        private readonly IGuildData guildData;

        public Guild Guild { get; set; }

        public DeleteModel(IGuildData guildData)
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
            var guild = guildData.Delete(guildId);
            guildData.Commit();

            if(guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            TempData["Message"] = $"{guild.Name} deleted";
            return RedirectToPage("./Index");
        }
    }
}
