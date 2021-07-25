using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MyForum.Core
{
    public class MyUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public int PrestigePoints { get; set; }
        public Rank Rank { get; set; }

        public int? ManagedGuildId { get; set; }
        public Guild ManagedGuild { get; set; }

        public IList<ChatMessage> ChatMessages { get; set; }
        public IList<Invitation> Invitations { get; set; }
        public IList<Guild> GuildsMembership { get; set; }
    }
}
