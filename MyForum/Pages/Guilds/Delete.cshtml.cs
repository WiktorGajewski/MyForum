using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Security.Claims;

namespace MyForum.Pages.Guilds
{
    [Authorize(Policy = "IsLeaderOrGuildmaster")]
    public class DeleteModel : PageModel
    {
        private readonly IGuildRepostiory guildData;
        private readonly IUserRepository userData;

        public Guild Guild { get; set; }

        public string currentUserId { get; set; }
        public string currentUserRank { get; set; }

        public DeleteModel(IGuildRepostiory guildData, IUserRepository userData, IHttpContextAccessor httpContextAccessor)
        {
            this.guildData = guildData;
            this.userData = userData;
            currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            currentUserRank = httpContextAccessor
                .HttpContext.User.FindFirst("Rank").Value;
        }

        public IActionResult OnGet(int guildId)
        {
            Guild = guildData.GetById(guildId);

            if(Guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            var managedGuildId = userData.GetManagedGuild(currentUserId)?.Id;

            if (currentUserRank == "Guildmaster" && Guild.Id != managedGuildId)     
            {
                TempData["Message"] = "You have no permission to edit this guild!";
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
