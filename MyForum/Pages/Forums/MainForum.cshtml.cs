using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Core;
using MyForum.Data.Interfaces;

namespace MyForum.Pages.Forums
{
    public class MainForumModel : PageModel
    {
        private readonly IMessageData messageData;

        private readonly int batchSize = 10;

        public IEnumerable<ChatMessage> Messages { get; set; }

        public string CurrentUserId { get; set; }

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Your message is empty!")]
        public string NewMessageContent { get; set; }

        public int PageNumber { get; set; }

        public int BatchSize => batchSize;

        public MainForumModel(IMessageData messageData, IHttpContextAccessor httpContextAccessor)
        {
            this.messageData = messageData;
            CurrentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public void OnGet(int PageNumber)
        {
            this.PageNumber = PageNumber;
            Messages = messageData.GetByGuildId(null, batchSize, batchSize*PageNumber);
        }

        public IActionResult OnPost()
        {

            if(ModelState.IsValid)
            {
                var newMessage = new ChatMessage()
                {
                    FromUserId = CurrentUserId,
                    Time = DateTime.Now,
                    Message = NewMessageContent
                };

                messageData.Add(newMessage);
                messageData.Commit();

                return RedirectToPage();
            }

            return Page();
        }
    }
}
