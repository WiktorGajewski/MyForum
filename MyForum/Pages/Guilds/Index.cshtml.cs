using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace MyForum.Pages.Guilds
{
    public class ListModel : PageModel
    {
        private readonly IGuildData guildData;

        [TempData]
        public string Message { get; set; }

        public IEnumerable<Guild> Guilds { get; set; } 

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public ListModel(IGuildData guildData)
        {
            this.guildData = guildData;
        }

        public void OnGet()
        {
            Guilds = guildData.GetByName(SearchTerm);
        }
    }
}
