using MyForum.Core;
using System.Collections.Generic;

namespace MyForum.Data.Interfaces
{
    public interface IMessageRepository
    {
        void Add(ChatMessage message);
        ChatMessage GetById(long messageId);
        IEnumerable<ChatMessage> GetByGuildId(int? guildId, int batchSize, int skip);
        void AddLike(long messageId, Like like);
        bool CheckIfLikeWasGiven(long messageId, string userId);
    }
}
