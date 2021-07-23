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
        private readonly IInvitationData invitationData;
        private readonly IUserData userData;
        private readonly IGuildData guildData;
        private readonly string currentUserId;
        private readonly int? managedGuildId;

        [BindProperty]
        public Invitation NewInvitation { get; set; }

        public string UserName { get; set; }

        public SendInvitationModel(IInvitationData invitationData, IUserData userData, IGuildData guildData,
            IHttpContextAccessor httpContextAccessor)
        {
            this.invitationData = invitationData;
            this.userData = userData;
            this.guildData = guildData;
            currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            managedGuildId = userData.GetManagedGuildId(currentUserId);
        }

        public IActionResult OnGet(string userId)
        {
            var user = userData.GetByIdWithGuilds(userId);
            
            if (user == null)
            {
                TempData["Message"] = "User was not found";
                return RedirectToPage("./NotFound");
            }

            if(managedGuildId == null)
            {
                TempData["Message"] = "You have no guild to invite to";
                return RedirectToPage("./NotFound");
            }

            var guild = guildData.GetById(managedGuildId.Value);

            if (user.GuildsMembership.Contains(guild))
            {
                TempData["Message"] = "User is already a member of your guild";
                return RedirectToPage("./NotFound");
            }

            if (invitationData.Find(user.Id, managedGuildId.Value) != null)
            {
                TempData["Message"] = "User has already been invited to your guild";
                return RedirectToPage("./NotFound");
            }

            

            NewInvitation = new Invitation
            {
                GuildId = managedGuildId.Value,
                UserId = user.Id,
                Message = $"You have been invited to the guild {guild.Name}"
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
            invitationData.Commit();

            TempData["Message"] = "Invitation sent";
            return RedirectToPage("./Index");
        }
    }
}
