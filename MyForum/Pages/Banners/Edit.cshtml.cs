using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data;

namespace MyForum.Pages.Banners
{
    public class EditModel : PageModel
    {
        private readonly IBannerData bannerData;

        [BindProperty]
        public Banner Banner { get; set; }

        public EditModel(IBannerData bannerData)
        {
            this.bannerData = bannerData;
        }

        public IActionResult OnGet(int? bannerId)
        {
            if(bannerId.HasValue)
            {
                Banner = bannerData.GetById(bannerId.Value);
            }
            else
            {
                Banner = new Banner();
            }
            
            if(Banner == null)
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

            if(Banner.Id > 0)
            {
                Banner = bannerData.Update(Banner);
            }
            else
            {
                Banner = bannerData.Add(Banner);
            }
            
            bannerData.Commit();
            TempData["Message"] = "Banner saved";
            return RedirectToPage("./Details", new { bannerId = Banner.Id });
        }
    }
}
