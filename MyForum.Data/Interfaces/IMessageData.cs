using MyForum.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyForum.Data.Interfaces
{
    public interface IMessageData
    {
        IEnumerable<ChatMessage> GetByGuildId(int? guildId);
        IEnumerable<ChatMessage> GetByGuildId(int? guildId, int batchSize, int skip);
        IEnumerable<ChatMessage> GetByUserId(string userId);
        ChatMessage GetById(int messageId);
        ChatMessage Add(ChatMessage message);
        int Commit();
    }
}
