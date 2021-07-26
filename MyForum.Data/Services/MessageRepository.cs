using Microsoft.EntityFrameworkCore;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyForum.Data.Services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MyForumDbContext _context;

        public MessageRepository(MyForumDbContext context)
        {
            this._context = context;
        }

        public void Add(ChatMessage newMessage)
        {
            _context.Add(newMessage);
            _context.SaveChanges();
        }

        public ChatMessage GetById(int messageId)
        {
            return _context.Messages.Find(messageId);
        }
        
        public IEnumerable<ChatMessage> GetByGuildId(int? guildId, int batchSize, int skip)
        {
            return _context.Messages
                .Where(m => m.GuildId == guildId)
                .OrderByDescending(m => m.Time)
                .Include(m => m.FromUser)           //with authors
                .Skip(skip)
                .Take(batchSize)
                .OrderBy(m => m.Time)
                .ToList();
        }
    }
}
