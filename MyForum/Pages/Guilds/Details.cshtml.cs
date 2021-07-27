using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Security.Claims;

namespace MyForum.Pages.Guilds
{
    public class DetailsModel : PageModel
    {
        private readonly IGuildRepostiory guildRepository;

        [TempData]
        public string Message { get; set; }

        public Guild Guild { get; set; }

        public string currentUserId { get; set; }

        public DetailsModel(IGuildRepostiory guildRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.guildRepository = guildRepository;
            currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public IActionResult OnGet(int guildId)
        {
            Guild = guildRepository.GetByIdWithMembersData(guildId);

            if(Guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            return Page();
        }

        public IActionResult OnPost(int guildId, string memberId)
        {
            Guild = guildRepository.GetByIdWithMembersData(guildId);

            if (Guild.GuildmasterId != currentUserId)
            {
                TempData["Message"] = "You have no permission to delete members of this guild!";
                return RedirectToPage("./NotFound");
            }

            guildRepository.RemoveMember(Guild.Id, memberId);

            TempData["Message"] = "Member removed from the Guild";
            return RedirectToPage();
        }
    }
}
