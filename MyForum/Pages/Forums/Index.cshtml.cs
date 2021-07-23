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
        private readonly IMessageData messageData;
        private readonly IGuildData guildData;
        private readonly IUserData userData;
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

        public IndexModel(IMessageData messageData, IGuildData guildData, IUserData userData,
            IHttpContextAccessor httpContextAccessor)
        {
            this.messageData = messageData;
            this.guildData = guildData;
            this.userData = userData;
            CurrentUserId = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public IActionResult OnGet(int PageNumber, int? guildId)
        {
            this.PageNumber = PageNumber;
            Messages = messageData.GetByGuildId(guildId, batchSize, batchSize * PageNumber);
            GuildId = guildId;

            if (guildId == null)
            {
                GuildName = "Main";
                return Page();
            }

            var guild = guildData.GetById(guildId.Value);

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

        public IActionResult OnPost()
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

                messageData.Add(newMessage);
                messageData.Commit();

                return RedirectToPage();
            }

            return Page();
        }

        private bool CheckAccess(Guild guild)
        {
            if(GuildId == null)
            {
                return true;
            }

            if (guild.GuildmasterId == null)            //no Guildmaster =>  guild is open
            {
                return true;
            }

            if(guild.GuildmasterId == CurrentUserId)    //Guidmaster always has access to his guild
            {
                return true;
            }

            var user = userData.GetByIdWithGuilds(CurrentUserId);

            if(user.GuildsMembership.Contains(guild))
            {
                return true;
            }

            return false;
        }
    }
}
