using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;

namespace MyForum.Pages.Users
{
    [Authorize(Policy = "IsLeader")]
    public class DeleteModel : PageModel
    {
        private readonly IUserRepository userData;

        public MyUser MyUser { get; set; }

        public DeleteModel(IUserRepository userData)
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

        public IActionResult OnPost(string userId)
        {
            userData.Delete(userId);

            TempData["Message"] = $"User deleted";
            return RedirectToPage("./Index");
        }
    }
}
