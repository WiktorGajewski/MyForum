using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyForum.Pages.Guilds
{
    [Authorize(Policy = "IsLeader")]
    public class ChangeGuildmasterModel : PageModel
    {
        private readonly IGuildRepostiory guildRepository;
        private readonly IUserRepository userRepository;

        public Guild Guild { get; set; }

        public MyUser CurrentGuildmaster { get; set; }

        public IEnumerable<SelectListItem> AvailableGuildmasters { get; set; }

        [BindProperty]
        public string NextGuildmasterId { get; set; }

        public ChangeGuildmasterModel(IGuildRepostiory guildRepository, IUserRepository userRepository)
        {
            this.guildRepository = guildRepository;
            this.userRepository = userRepository;
        }

        public IActionResult OnGet(int guildId)
        {
            Guild = guildRepository.GetById(guildId);

            if(Guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            CurrentGuildmaster = userRepository.GetById(Guild?.GuildmasterId);
            var availableGuildmasters = userRepository.GetGuildmastersWithoutGuild();

            IEnumerable<SelectListItem> enumerable = availableGuildmasters.Select(x =>
                           new SelectListItem()
                           {
                               Text = x?.UserName,
                               Value = x?.Id
                           });
            AvailableGuildmasters = enumerable;

            return Page();
        }

        public IActionResult OnPost(int guildId)
        {
            Guild = guildRepository.GetById(guildId);
            
            if(Guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            guildRepository.RemoveGuildmaster(guildId, Guild.GuildmasterId);
            guildRepository.AssignGuildmaster(guildId, NextGuildmasterId);
            guildRepository.AddMember(guildId, NextGuildmasterId);

            TempData["Message"] = "Guildmaster changed";
            return RedirectToPage("./Index");
        }
    }
}
