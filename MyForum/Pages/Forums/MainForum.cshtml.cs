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
        private readonly IHttpContextAccessor httpContextAccessor;

        public IEnumerable<ChatMessage> Messages { get; set; }

        public string CurrentUserId { get; set; }

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Your message is empty!")]
        public string NewMessageContent { get; set; }

        public int PageNumber { get; set; }

        public MainForumModel(IMessageData messageData, IHttpContextAccessor httpContextAccessor)
        {
            this.messageData = messageData;
            this.httpContextAccessor = httpContextAccessor;
            CurrentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public void OnGet(int PageNumber)
        {
            Messages = messageData.GetLast10(null, PageNumber);
            this.PageNumber = PageNumber;
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

                messageData.AddNewMessage(newMessage);
                messageData.Commit();

                return RedirectToPage();
            }

            return Page();
        }
    }
}
