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
        private readonly IGuildRepostiory guildData;
        private readonly IUserRepository userData;

        public Guild Guild { get; set; }

        public MyUser CurrentGuildmaster { get; set; }

        public IEnumerable<SelectListItem> AvailableGuildmasters { get; set; }

        [BindProperty]
        public string NextGuildmasterId { get; set; }

        public ChangeGuildmasterModel(IGuildRepostiory guildData, IUserRepository userData)
        {
            this.guildData = guildData;
            this.userData = userData;
        }

        public IActionResult OnGet(int guildId)
        {
            Guild = guildData.GetById(guildId);

            if(Guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            CurrentGuildmaster = userData.GetById(Guild?.GuildmasterId);
            var availableGuildmasters = userData.GetGuildmastersWithoutGuild();

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
            Guild = guildData.GetById(guildId);
            
            if(Guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("./NotFound");
            }

            guildData.RemoveGuildmaster(guildId, Guild.GuildmasterId);
            guildData.AssignGuildmaster(guildId, NextGuildmasterId);
            guildData.AddMember(guildId, NextGuildmasterId);

            TempData["Message"] = "Guildmaster changed";
            return RedirectToPage("./Index");
        }
    }
}
