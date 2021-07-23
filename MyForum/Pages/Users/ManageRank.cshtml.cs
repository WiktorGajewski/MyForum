using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;

namespace MyForum.Pages.Users
{
    [Authorize(Policy = "IsLeader")]
    public class ManageRankModel : PageModel
    {
        private readonly IUserData userData;

        [BindProperty]
        public MyUser MyUser { get; set; }

        public int DaysOfService { get; set; }

        public int MaxPossibleRank { get; set; }
        public int MinPossibleRank { get; set; }
        public int UserRank { get; set; }

        public ManageRankModel(IUserData userData)
        {
            this.userData = userData;
            MaxPossibleRank = (int)Enum.GetValues(typeof(Rank)).Cast<Rank>().Max();
            MinPossibleRank = (int)Enum.GetValues(typeof(Rank)).Cast<Rank>().Min();
        }

        public IActionResult OnGet(string userId)
        {
            MyUser = userData.GetById(userId);

            if(MyUser == null)
            {
                TempData["Message"] = "User was not found";
                return RedirectToPage("./NotFound");
            }

            UserRank = (int)MyUser.Rank;
            
            var days = (DateTime.Now - MyUser.RegistrationDate).TotalDays;
            DaysOfService = (int)Math.Round(days);

            return Page();
        }

        public IActionResult OnPostRankUp()
        {
            UserRank = (int)MyUser.Rank;

            if ((UserRank + 1) > MaxPossibleRank)
            {
                TempData["Message"] = $"{MyUser.UserName} rank could not be raised";
                return RedirectToPage("./Index");
            }

            UserRank++;
            var newRank = (Rank)UserRank;
            userData.SetUpNewRank(MyUser.Id, newRank);
            userData.Commit();

            TempData["Message"] = $"{MyUser.UserName} rank has been raised";
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostRankDown()
        {
            UserRank = (int)MyUser.Rank;

            if ((UserRank - 1) < MinPossibleRank)
            {
                TempData["Message"] = $"{MyUser.UserName} rank could not be lowered";
                return RedirectToPage("./Index");
            }

            UserRank--;
            var newRank = (Rank)UserRank;
            userData.SetUpNewRank(MyUser.Id, newRank);
            userData.Commit();

            TempData["Message"] = $"{MyUser.UserName} rank has been lowered";
            return RedirectToPage("./Index");
        }
    }
}
