using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IMessageRepository
    {
        void Add(ChatMessage message);
        ChatMessage GetById(int messageId);
        IEnumerable<ChatMessage> GetByGuildId(int? guildId, int batchSize, int skip);
    }
}
