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
        private readonly IGuildRepostiory guildRepository;
        private readonly IUserRepository userRepository;

        public Guild Guild { get; set; }

        public string currentUserId { get; set; }
        public string currentUserRank { get; set; }

        public DeleteModel(IGuildRepostiory guildRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.guildRepository = guildRepository;
            this.userRepository = userRepository;
            currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            currentUserRank = httpContextAccessor
                .HttpContext.User.FindFirst("Rank").Value;
        }

        public IActionResult OnGet(int guildId)
        {
            Guild = guildRepository.GetById(guildId);

            if(Guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            var managedGuildId = userRepository.GetManagedGuild(currentUserId)?.Id;

            if (currentUserRank == "Guildmaster" && Guild.Id != managedGuildId)     
            {
                TempData["Message"] = "You have no permission to edit this guild!";
                return RedirectToPage("./NotFound");
            }

            return Page();
        }

        public IActionResult OnPost(int guildId)
        {
            Guild = guildRepository.GetById(guildId);

            if (Guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            guildRepository.RemoveGuildmaster(guildId, Guild.GuildmasterId);
            guildRepository.Delete(guildId);

            TempData["Message"] = $"Guild deleted";
            return RedirectToPage("./Index");
        }
    }
}
