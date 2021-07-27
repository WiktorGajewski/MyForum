using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MyForum.Pages.Guilds
{
    public class IndexModel : PageModel
    {
        private readonly IGuildRepostiory guildRepository;
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

        public IndexModel(IGuildRepostiory guildRepository, IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository)
        {
            this.guildRepository = guildRepository;
            var currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ManagedGuildId = userRepository.GetManagedGuild(currentUserId)?.Id;
        }

        public void OnGet(int PageNumber)
        {
            this.PageNumber = PageNumber;
            Guilds = guildRepository.GetByName(SearchTerm, BatchSize, BatchSize*PageNumber);
            GuildsCount = guildRepository.CountGuilds(SearchTerm);
        }
    }
}
