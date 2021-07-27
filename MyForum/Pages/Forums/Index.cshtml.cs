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
    public class IndexModel : PageModel
    {
        private readonly IMessageRepository messageRepository;
        private readonly IGuildRepostiory guildRepository;
        private readonly IUserRepository userRepository;
        private readonly int batchSize = 10;

        public IEnumerable<ChatMessage> Messages { get; set; }

        public string CurrentUserId { get; set; }

        [BindProperty]
        public int? GuildId { get; set; }

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Your message is empty!")]
        public string NewMessageContent { get; set; }

        public int PageNumber { get; set; }

        public string GuildName { get; set; }

        public int BatchSize => batchSize;

        public IndexModel(IMessageRepository messageRepository, IGuildRepostiory guildRepository, 
            IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.messageRepository = messageRepository;
            this.guildRepository = guildRepository;
            this.userRepository = userRepository;
            CurrentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public IActionResult OnGet(int PageNumber, int? guildId)
        {
            this.PageNumber = PageNumber;
            Messages = messageRepository.GetByGuildId(guildId, batchSize, batchSize * PageNumber);
            GuildId = guildId;

            if (guildId == null)
            {
                GuildName = "Main";
                return Page();
            }

            var guild = guildRepository.GetById(guildId.Value);

            if (guild == null)
            {
                TempData["Message"] = "Guild was not found";
                return RedirectToPage("../Guilds/NotFound");
            }

            if(!CheckAccess(guild))
            {
                TempData["Message"] = "You are not a member of this group!";
                return RedirectToPage("../Guilds/NotFound");
            }

            GuildName = guild?.Name;

            return Page();
        }

        public IActionResult OnPost(int PageNumber, int? guildId)
        {

            if (ModelState.IsValid)
            {
                var newMessage = new ChatMessage()
                {
                    FromUserId = CurrentUserId,
                    GuildId = this.GuildId,
                    Time = DateTime.Now,
                    Message = NewMessageContent,
                };

                messageRepository.Add(newMessage);

                return RedirectToPage();
            }

            return OnGet(PageNumber, guildId);
        }

        public IActionResult OnPostGiveLike(int PageNumber, int? guildId, long messageId)
        {
            if(messageRepository.CheckIfLikeWasGiven(messageId, CurrentUserId))
            {
                return OnGet(PageNumber, guildId);
            }

            var newLike = new Like()
            {
                FromUserId = CurrentUserId,
                MessageId = messageId
            };

            messageRepository.AddLike(messageId, newLike);

            return RedirectToPage();
        }

        private bool CheckAccess(Guild guild)
        {
            if(GuildId == null)
            {
                return true;
            }

            if(guild.GuildmasterId == CurrentUserId)    //Guidmaster always has access to his guild
            {
                return true;
            }

            var user = userRepository.GetByIdWithMembershipData(CurrentUserId);

            if(user.GuildsMembership.Contains(guild))
            {
                return true;
            }

            return false;
        }
    }
}
