using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;

namespace MyForum.Pages.Users
{
    [Authorize(Policy = "IsLeader")]
    public class DeleteModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly IGuildRepostiory guildRepository;

        public MyUser MyUser { get; set; }

        public DeleteModel(IUserRepository userRepository, IGuildRepostiory guildRepository)
        {
            this.userRepository = userRepository;
            this.guildRepository = guildRepository;
        }

        public IActionResult OnGet(string userId)
        {
            MyUser = userRepository.GetById(userId);

            if(MyUser == null)
            {
                TempData["Message"] = "User was not found";
                return RedirectToPage("./NotFound");
            }

            return Page();
        }

        public IActionResult OnPost(string userId)
        {
            MyUser = userRepository.GetById(userId);

            if(MyUser?.ManagedGuildId != null)
            {
                guildRepository.RemoveGuildmaster(MyUser.ManagedGuildId.Value, MyUser?.Id);
            }
            
            userRepository.Delete(userId);

            TempData["Message"] = $"User deleted";
            return RedirectToPage("./Index");
        }
    }
}
