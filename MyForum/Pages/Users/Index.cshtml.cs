using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;

namespace MyForum.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserData userData;

        [TempData]
        public string Message { get; set; }

        public IEnumerable<MyUser> MyUsers { get; set; }

        public IndexModel(IUserData userData)
        {
            this.userData = userData;
        }

        public void OnGet()
        {
            MyUsers = userData.GetByUsername(null);
        }
    }
}
