using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MyForum.Core
{
    public class ChatMessage
    {
        public long Id { get; set; }

        [Required]
        public string Message { get; set; }
        public DateTime Time { get; set; }

        public string FromUserId { get; set; }
        public virtual MyUser FromUser { get; set; }

        public int? GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public IList<Like> Likes { get; set; }
        public int NumberOfLikes { get; set; }
    }
}
