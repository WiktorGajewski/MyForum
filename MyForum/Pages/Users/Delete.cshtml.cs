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
        private readonly IGuildRepostiory guildData;

        public MyUser MyUser { get; set; }

        public DeleteModel(IUserRepository userData, IGuildRepostiory guildData)
        {
            this.userData = userData;
            this.guildData = guildData;
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
            MyUser = userData.GetById(userId);

            if(MyUser?.ManagedGuildId != null)
            {
                guildData.RemoveGuildmaster(MyUser.ManagedGuildId.Value, MyUser?.Id);
            }
            
            userData.Delete(userId);

            TempData["Message"] = $"User deleted";
            return RedirectToPage("./Index");
        }
    }
}
