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
        private readonly IInvitationRepository invitationRepository;
        private readonly IGuildRepostiory guildRepository;

        public string CurrentUserId { get; set; }

        public IEnumerable<Invitation> Invitations { get; set; }

        public IndexModel(IInvitationRepository invitationRepository, IGuildRepostiory guildRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.invitationRepository = invitationRepository;
            this.guildRepository = guildRepository;
            CurrentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public void OnGet()
        {
            Invitations = invitationRepository.GetByUserId(CurrentUserId);
        }

        public IActionResult OnPostAccept(int guildId)
        {
            guildRepository.AddMember(guildId, CurrentUserId);
            invitationRepository.Delete(CurrentUserId, guildId);

            return RedirectToPage();
        }

        public IActionResult OnPostDecline(int guildId)
        {
            invitationRepository.Delete(CurrentUserId, guildId);

            return RedirectToPage();
        }
    }
}
