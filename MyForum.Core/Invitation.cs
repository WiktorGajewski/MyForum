using System.ComponentModel.DataAnnotations;

namespace MyForum.Core
{
    public class Invitation
    {
        public string UserId { get; set; }
        public MyUser User { get; set; }
        public int GuildId { get; set; }
        public Guild Guild { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
