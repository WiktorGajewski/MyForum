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

        private readonly int batchSize = 4;

        [TempData]
        public string Message { get; set; }

        public int GuildsCount { get; set; }

        public int PageNumber { get; set; }

        public IEnumerable<Guild> Guilds { get; set; } 

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public int BatchSize => batchSize;

        public IndexModel(IGuildData guildData)
        {
            this.guildData = guildData;
            GuildsCount = guildData.CountGuilds();
        }

        public void OnGet(int PageNumber)
        {
            this.PageNumber = PageNumber;
            Guilds = guildData.GetByName(SearchTerm, BatchSize, BatchSize*PageNumber);
        }
    }
}
