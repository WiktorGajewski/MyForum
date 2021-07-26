
namespace MyForum.Core
{
    public class Like
    {
        public string FromUserId { get; set; }
        public MyUser FromUser { get; set; }
        public long MessageId { get; set; }
        public ChatMessage Message { get; set; }
    }
}
