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
        private readonly IUserRepository userData;

        [TempData]
        public string Message { get; set; }

        public MyUser MyUser { get; set; }

        public DetailsModel(IUserRepository userData)
        {
            this.userData = userData;
        }

        public IActionResult OnGet(string userId)
        {
            MyUser = userData.GetById(userId);

            if(MyUser == null)
            {
                TempData["Message"] = "User was not found";
                return RedirectToPage("./NotFound");
            }

            return Page();
        }
    }
}
