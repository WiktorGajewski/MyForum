using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Security.Claims;

namespace MyForum.Pages.Users
{

    [Authorize(Policy = "IsGuildmaster")]
    public class SendInvitationModel : PageModel
    {
        private readonly IInvitationRepository invitationData;
        private readonly IUserRepository userData;
        private readonly IGuildRepostiory guildData;
        private readonly string currentUserId;
        private readonly Guild managedGuild;

        [BindProperty]
        public Invitation NewInvitation { get; set; }

        public string UserName { get; set; }

        public SendInvitationModel(IInvitationRepository invitationData, IUserRepository userData, IGuildRepostiory guildData,
            IHttpContextAccessor httpContextAccessor)
        {
            this.invitationData = invitationData;
            this.userData = userData;
            this.guildData = guildData;
            currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            managedGuild = userData.GetManagedGuild(currentUserId);
        }

        public IActionResult OnGet(string userId)
        {
            var user = userData.GetByIdWithMembershipData(userId);
            
            if (user == null)
            {
                TempData["Message"] = "User was not found";
                return RedirectToPage("./NotFound");
            }

            if(managedGuild == null)
            {
                TempData["Message"] = "You have no guild to invite to";
                return RedirectToPage("./NotFound");
            }

            if (user.GuildsMembership.Contains(managedGuild))
            {
                TempData["Message"] = "User is already a member of your guild";
                return RedirectToPage("./NotFound");
            }

            if (invitationData.Get(user.Id, managedGuild.Id) != null)
            {
                TempData["Message"] = "User has already been invited to your guild";
                return RedirectToPage("./NotFound");
            }

            NewInvitation = new Invitation
            {
                GuildId = managedGuild.Id,
                UserId = user.Id,
                Message = $"You have been invited to the guild {managedGuild.Name}"
            };

            UserName = user.UserName;

            return Page();
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            invitationData.Add(NewInvitation);

            TempData["Message"] = "Invitation sent";
            return RedirectToPage("./Index");
        }
    }
}
