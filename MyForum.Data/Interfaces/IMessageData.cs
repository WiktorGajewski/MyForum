using MyForum.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyForum.Data.Interfaces
{
    public interface IMessageData
    {
        IEnumerable<ChatMessage> GetByGuildId(int? guildId);
        IEnumerable<ChatMessage> GetByUserId(string userId);
        ChatMessage GetById(int messageId);
        ChatMessage AddNewMessage(ChatMessage message);
        IEnumerable<ChatMessage> GetLast10(int? guildId, int skip);
        int Commit();
    }
}
