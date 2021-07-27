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
        private readonly IUserRepository userRepository;

        public MessageRepository(MyForumDbContext context, IUserRepository userRepository)
        {
            this._context = context;
            this.userRepository = userRepository;
        }

        public void Add(ChatMessage newMessage)
        {
            _context.Add(newMessage);
            _context.SaveChanges();
        }

        public ChatMessage GetById(long messageId)
        {
            return _context.Messages
                .Include(m => m.Likes)
                .Include(m => m.FromUser)
                .FirstOrDefault(m => m.Id == messageId);
        }
        
        public IEnumerable<ChatMessage> GetByGuildId(int? guildId, int batchSize, int skip)
        {
            return _context.Messages
                .Where(m => m.GuildId == guildId)
                .OrderByDescending(m => m.Time)
                .Include(m => m.FromUser)           //with authors
                .Include(m => m.Likes)              //and likes
                .Skip(skip)
                .Take(batchSize)
                .OrderBy(m => m.Time)
                .ToList();
        }

        public void AddLike(long messageId, Like like)
        {
            var message = GetById(messageId);

            if(message == null)
            {
                return;
            }

            message.Likes.Add(like);
            message.NumberOfLikes++;

            if(message.FromUser != null)
            {
                message.FromUser.PrestigePoints++;
            }

            _context.SaveChanges();
        }

        public bool CheckIfLikeWasGiven(long messageId, string userId)
        {
            var message = GetById(messageId);
            var user = userRepository.GetById(userId);

            if (message != null && user != null)
            {
               var like = _context.Likes.Find(user.Id, message.Id);

                return like != null;
            }

            return false;
        }
    }
}
