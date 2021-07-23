using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyForum.Core
{
    public class Guild
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        [ForeignKey("Guildmaster")]
        public string GuildmasterId { get; set; }
        public MyUser Guildmaster { get; set; }
        
        public IList<ChatMessage> ForumChatMessages { get; set; }
        public IList<MyUser> Members { get; set; }
        public IList<Invitation> Invitations { get; set; }
    }
}
