using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MyForum.Data.Interfaces;

namespace MyForum.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IInvitationRepository invitationRepository;

        public bool UserHasNewInvitations { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, IInvitationRepository invitationRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.invitationRepository = invitationRepository;
        }

        public void OnGet()
        {
            if(User.Identity.IsAuthenticated)
            {
                var currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                UserHasNewInvitations = invitationRepository.IsUserHavingAnyInvitation(currentUserId);
            }
            else
            {
                UserHasNewInvitations = false;
            }
        }
    }
}
