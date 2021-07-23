using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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

        public int? ManagedGuildId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public int BatchSize => batchSize;

        public IndexModel(IGuildData guildData, IHttpContextAccessor httpContextAccessor, IUserData userData)
        {
            this.guildData = guildData;
            var currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ManagedGuildId = userData.GetManagedGuildId(currentUserId);
        }

        public void OnGet(int PageNumber)
        {
            this.PageNumber = PageNumber;
            Guilds = guildData.GetByName(SearchTerm, BatchSize, BatchSize*PageNumber);
            GuildsCount = guildData.CountGuilds(SearchTerm);
        }
    }
}
