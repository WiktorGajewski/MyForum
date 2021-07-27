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
        private readonly IGuildRepostiory guildRepository;
        private readonly IUserRepository userRepository;

        [BindProperty]
        public Guild Guild { get; set; }

        [ViewData]
        public string Title { get; set; }

        public string currentUserId{ get; set; }

        public EditModel(IGuildRepostiory guildRepository, IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository)
        {
            this.guildRepository = guildRepository;
            this.userRepository = userRepository;
            currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public IActionResult OnGet(int? guildId)
        {
            Guild = userRepository.GetManagedGuild(currentUserId);

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

            if(guildRepository.CheckNameUnique(Guild.Name))
            {
                ModelState.AddModelError("Guild.Name", "Name is already in use");
                return OnGet(Guild.Id == 0 ? null : Guild.Id);
            }

            if(Guild.Id > 0)
            {
                guildRepository.Update(Guild);
            }
            else
            {
                guildRepository.Add(Guild);

                var guildmasterId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                guildRepository.AssignGuildmaster(Guild.Id, guildmasterId);
                guildRepository.AddMember(Guild.Id, guildmasterId);
            }
            
            TempData["Message"] = "Guild saved";
            return RedirectToPage("./Details", new { guildId = Guild.Id });
        }
    }
}
