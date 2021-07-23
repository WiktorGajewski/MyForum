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
    public class EditModel : PageModel
    {
        private readonly IGuildData guildData;
        private readonly IHttpContextAccessor httpContextAccessor;

        [BindProperty]
        public Guild Guild { get; set; }

        [ViewData]
        public string Title { get; set; }

        public int? ManagedGuildId { get; set; }

        public EditModel(IGuildData guildData, IHttpContextAccessor httpContextAccessor, IUserData userData)
        {
            this.guildData = guildData;
            this.httpContextAccessor = httpContextAccessor;
            var currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ManagedGuildId = userData.GetManagedGuildId(currentUserId);
        }

        public IActionResult OnGet(int? guildId)
        {
            if (!CheckPermissionsToEdit(guildId))
            {
                TempData["Message"] = "You have no permission to edit this guild!";
                return RedirectToPage("./NotFound");
            }

            if (guildId.HasValue)
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
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!CheckPermissionsToEdit(Guild?.Id))
            {
                TempData["Message"] = "You have no permission to edit this guild!";
                return RedirectToPage("./NotFound");
            }

            if (!ModelState.IsValid)
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
                guildData.Commit();

                if (User.FindFirst("Rank").Value == "Guildmaster")
                {
                    guildData.AssignGuildmaster(Guild.Id, User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
            }
            
            guildData.Commit();
            TempData["Message"] = "Guild saved";
            return RedirectToPage("./Details", new { guildId = Guild.Id });
        }

        private bool CheckPermissionsToEdit(int? guildId)
        {
            if(httpContextAccessor.HttpContext
                .User.FindFirst("Rank").Value == "Leader")          //Leader can edit any Guild
            {
                return true;
            }

            if(ManagedGuildId == (guildId == 0 ? null : guildId))   //Guildmaster can edit only his own Guild (ManagedGuildId == Guild.Id)
            {                                                       //or create new one if has none (ManagedGuildId == null && Guild.Id == null)
                return true;
            }

            return false;
        }
    }
}
