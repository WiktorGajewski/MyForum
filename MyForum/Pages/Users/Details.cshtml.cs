using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;

namespace MyForum.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly IUserData userData;

        [TempData]
        public string Message { get; set; }

        public new User User { get; set; }

        public DetailsModel(IUserData userData)
        {
            this.userData = userData;
        }

        public IActionResult OnGet(int userId)
        {
            User = userData.GetById(userId);

            if(User == null)
            {
                return RedirectToPage("./NotFound");
            }

            return Page();
        }
    }
}
