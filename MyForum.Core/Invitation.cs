
namespace MyForum.Core
{
    public class Invitation
    {
        public string UserId { get; set; }
        public MyUser User { get; set; }
        public int GuildId { get; set; }
        public Guild Guild { get; set; }
        public string Message { get; set; }
    }
}
