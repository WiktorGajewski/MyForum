using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyForum.Pages.Guilds
{
    public class IndexModel : PageModel
    {
        private readonly IGuildData guildData;

        [TempData]
        public string Message { get; set; }

        public IEnumerable<Guild> Guilds { get; set; } 

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public IndexModel(IGuildData guildData)
        {
            this.guildData = guildData;
        }

        public void OnGet()
        {
            Guilds = guildData.GetByName(SearchTerm);
        }
    }
}
