using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;

namespace MyForum.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly IUserRepository userRepository;

        [TempData]
        public string Message { get; set; }

        public MyUser MyUser { get; set; }

        public DetailsModel(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
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
    }
}
