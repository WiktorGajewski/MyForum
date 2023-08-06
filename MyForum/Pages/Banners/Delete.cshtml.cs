using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data;

namespace MyForum.Pages.Banners
{
    public class DeleteModel : PageModel
    {
        private readonly IBannerData bannerData;

        public Banner Banner { get; set; }

        public DeleteModel(IBannerData bannerData)
        {
            this.bannerData = bannerData;
        }

        public IActionResult OnGet(int bannerId)
        {
            Banner = bannerData.GetById(bannerId);
            if(Banner == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost(int bannerId)
        {
            var banner = bannerData.Delete(bannerId);
            bannerData.Commit();

            if(banner == null)
            {
                return RedirectToPage("./NotFound");
            }

            TempData["Message"] = $"{banner.Name} deleted";
            return RedirectToPage("./List");
        }
    }
}
