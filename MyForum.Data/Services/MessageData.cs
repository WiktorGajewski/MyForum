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

        public ChatMessage AddNewMessage(ChatMessage newMessage)
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

        public IEnumerable<ChatMessage> GetByGuildId(int? guildId) // guildId == null <- Main Forum
        {
            var list = db.Messages
                .Where(m => m.GuildId == guildId)
                .Include(m => m.FromUser)           //with authors
                .OrderBy(m => m.Time)
                .ToList();

            return list;
        }

        public IEnumerable<ChatMessage> GetByUserId(string userId)
        {
            var list = db.Messages
                .Where(m => m.FromUserId == userId)
                .Include(m => m.GuildForum)         //with data about which forum it was
                .OrderBy(m => m.Time)
                .ToList();

            return list;
        }

        public IEnumerable<ChatMessage> GetLast10(int? guildId, int skip = 0)
        {
            var list = db.Messages
                .Where(m => m.GuildId == guildId)
                .OrderByDescending(m => m.Time)
                .Include(m => m.FromUser)
                .Skip(10*skip)
                .Take(10)
                .OrderBy(m => m.Time)
                .ToList();

            return list;
        }
    }
}
