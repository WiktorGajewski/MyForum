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
    public class EditModel : PageModel
    {
        private readonly IUserData userData;

        [BindProperty]
        public new User User { get; set; }

        [ViewData]
        public string Title { get; set; }

        public EditModel(IUserData userData)
        {
            this.userData = userData;
        }

        public IActionResult OnGet(int? userId)
        {
            if(userId.HasValue)
            {
                User = userData.GetById(userId.Value);
                Title = "Edit";
            }
            else
            {
                User = new User();
                Title = "Create";
            }
            
            if(User == null)
            {
                return RedirectToPage("./NotFound");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }


            if(User.Id > 0)
            {
                userData.Update(User);
            }
            else
            {
                User.RegistrationDate = DateTime.Now;
                User.PrestigePoints = 0;
                userData.Add(User);
            }

            userData.Commit();
            TempData["Message"] = "User saved";
            return RedirectToPage("./Details", new { userId = User.Id });
        }
    }
}
