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
        private readonly IUserData userData;

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

        public IndexModel(IUserData userData, IHttpContextAccessor httpContextAccessor)
        {
            this.userData = userData;
            var currentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ManagedGuildId = userData.GetManagedGuildId(currentUserId);
        }

        public void OnGet(int PageNumber)
        {
            this.PageNumber = PageNumber;
            MyUsers = userData.GetByUsername(SearchTerm, batchSize, batchSize*PageNumber);
            UsersCount = userData.CountUsers(SearchTerm);
        }
    }
}
