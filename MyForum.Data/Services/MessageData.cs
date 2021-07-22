using Microsoft.EntityFrameworkCore;
using MyForum.Core;
using MyForum.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Data.Services
{
    public class MessageData : IMessageData
    {
        private readonly MyForumDbContext db;

        public MessageData(MyForumDbContext db)
        {
            this.db = db;
        }

        public ChatMessage Add(ChatMessage newMessage)
        {
            db.Add(newMessage);
            return newMessage;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public ChatMessage GetById(int messageId)
        {
            return db.Messages.Find(messageId);
        }

        public IEnumerable<ChatMessage> GetByUserId(string userId)
        {
            var query = db.Messages
                .Where(m => m.FromUserId == userId)
                .Include(m => m.GuildForum)         //with data about which forum it was
                .OrderBy(m => m.Time)
                .ToList();

            return query;
        }

        public IEnumerable<ChatMessage> GetByGuildId(int? guildId) // guildId == null <- Main Forum
        {
            var query = db.Messages
                .Where(m => m.GuildId == guildId)
                .Include(m => m.FromUser)           //with authors
                .OrderBy(m => m.Time)
                .ToList();

            return query;
        }
        
        public IEnumerable<ChatMessage> GetByGuildId(int? guildId, int batchSize, int skip)
        {
            var query = db.Messages
                .Where(m => m.GuildId == guildId)
                .OrderByDescending(m => m.Time)
                .Include(m => m.FromUser)
                .Skip(skip)
                .Take(batchSize)
                .OrderBy(m => m.Time)
                .ToList();

            return query;
        }
    }
}
