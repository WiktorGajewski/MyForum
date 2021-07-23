using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;

namespace MyForum.Pages.Invitations
{
    public class IndexModel : PageModel
    {
        private readonly IInvitationData invitationData;
        private readonly IGuildData guildData;

        public string CurrentUserId { get; set; }

        public IEnumerable<Invitation> Invitations { get; set; }

        public IndexModel(IInvitationData invitationData, IGuildData guildData, IHttpContextAccessor httpContextAccessor)
        {
            this.invitationData = invitationData;
            this.guildData = guildData;
            CurrentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public void OnGet()
        {
            Invitations = invitationData.GetByUserId(CurrentUserId);
        }

        public IActionResult OnPostAccept(int guildId)
        {
            guildData.AddMember(guildId, CurrentUserId);
            invitationData.Delete(CurrentUserId, guildId);
            invitationData.Commit();

            return RedirectToPage();
        }

        public IActionResult OnPostDecline(int guildId)
        {
            invitationData.Delete(CurrentUserId, guildId);
            invitationData.Commit();

            return RedirectToPage();
        }
    }
}
