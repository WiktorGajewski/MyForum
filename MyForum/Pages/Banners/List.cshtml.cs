using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace MyForum.Pages.Banners
{
    public class ListModel : PageModel
    {
        private readonly IBannerData bannerData;

        [TempData]
        public string Message { get; set; }

        public IEnumerable<Banner> Banners { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public ListModel(IBannerData bannerData)
        {
            this.bannerData = bannerData;
        }

        public void OnGet()
        {
            Banners = bannerData.GetByName(SearchTerm);
        }
    }
}
