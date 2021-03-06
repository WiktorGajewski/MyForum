using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;

namespace MyForum.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserRepository userRepository;

        private readonly int batchSize = 4;

        [TempData]
        public string Message { get; set; }

        public int UsersCount { get; set; }

        public int PageNumber { get; set; }

        public IEnumerable<MyUser> MyUsers { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public int? ManagedGuildId { get; set; }

        public int BatchSize => batchSize;

        public IndexModel(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            var currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ManagedGuildId = userRepository.GetManagedGuild(currentUserId)?.Id;
        }

        public void OnGet(int PageNumber)
        {
            this.PageNumber = PageNumber;
            MyUsers = userRepository.GetByUserName(SearchTerm, batchSize, batchSize*PageNumber);
            UsersCount = userRepository.CountUsers(SearchTerm);
        }
    }
}
