using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Security.Claims;

namespace MyForum.Pages.Guilds
{

    [Authorize(Policy = "IsGuildmaster")]
    public class EditModel : PageModel
    {
        private readonly IGuildRepostiory guildData;
        private readonly IUserRepository userData;

        [BindProperty]
        public Guild Guild { get; set; }

        [ViewData]
        public string Title { get; set; }

        public string currentUserId{ get; set; }

        public EditModel(IGuildRepostiory guildData, IHttpContextAccessor httpContextAccessor, IUserRepository userData)
        {
            this.guildData = guildData;
            this.userData = userData;
            currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public IActionResult OnGet(int? guildId)
        {
            Guild = userData.GetManagedGuild(currentUserId);

            if(Guild == null && guildId == null)
            {
                Guild = new Guild();
                Title = "Create";
            }
            else if(Guild?.Id != guildId)
            {
                TempData["Message"] = "You have no permission to edit this guild!";
                return RedirectToPage("./NotFound");
            }
            else
            {
                Title = "Edit";
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(guildData.CheckNameUnique(Guild.Name))
            {
                ModelState.AddModelError("Guild.Name", "Name is already in use");
                return OnGet(Guild.Id == 0 ? null : Guild.Id);
            }

            if(Guild.Id > 0)
            {
                guildData.Update(Guild);
            }
            else
            {
                guildData.Add(Guild);

                var guildmasterId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                guildData.AssignGuildmaster(Guild.Id, guildmasterId);
                guildData.AddMember(Guild.Id, guildmasterId);
            }
            
            TempData["Message"] = "Guild saved";
            return RedirectToPage("./Details", new { guildId = Guild.Id });
        }
    }
}
