using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MyForum.Data.Interfaces;
using System.Linq;

namespace MyForum.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IInvitationRepository invitationData;

        public bool UserHasNewInvitations { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, IInvitationRepository invitationData)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.invitationData = invitationData;
        }

        public void OnGet()
        {
            if(User.Identity.IsAuthenticated)
            {
                var currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                UserHasNewInvitations = invitationData.IsUserHavingAnyInvitation(currentUserId);
            }
            else
            {
                UserHasNewInvitations = false;
            }
        }
    }
}
