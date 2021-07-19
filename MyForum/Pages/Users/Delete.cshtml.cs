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
    public class DeleteModel : PageModel
    {
        private readonly IUserData userData;

        public MyUser MyUser { get; set; }

        public DeleteModel(IUserData userData)
        {
            this.userData = userData;
        }

        public IActionResult OnGet(string userId)
        {
            MyUser = userData.GetById(userId);

            if(MyUser == null)
            {
                return RedirectToPage("./NotFound");
            }

            return Page();
        }

        public IActionResult OnPost(string userId)
        {
            var user = userData.Delete(userId);
            userData.Commit();

            if(user == null)
            {
                return RedirectToPage("./NotFound");
            }

            TempData["Message"] = $"{user.UserName} deleted";
            return RedirectToPage("./Index");
        }
    }
}
