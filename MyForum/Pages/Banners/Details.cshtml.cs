using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data;

namespace MyForum.Pages.Banners
{
    public class DetailsModel : PageModel
    {
        private readonly IBannerData bannerData;

        [TempData]
        public string Message { get; set; }

        public Banner Banner { get; set; }

        public DetailsModel(IBannerData bannerData)
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
    }
}
