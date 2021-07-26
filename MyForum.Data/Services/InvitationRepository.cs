using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyForum.Data.Services
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly MyForumDbContext _context;

        public InvitationRepository(MyForumDbContext context)
        {
            this._context = context;
        }

        public void Add(Invitation newInvitation)
        {
            _context.Add(newInvitation);
            _context.SaveChanges();
        }

        public void Delete(string userId, int guildId)
        {
            var invitation = Get(userId, guildId);

            if(invitation != null)
            {
                _context.Invitations.Remove(invitation);
                _context.SaveChanges();
            }
        }

        public Invitation Get(string userId, int guildId)
        {
            return _context.Invitations.Find(guildId, userId);
        }

        public IEnumerable<Invitation> GetByUserId(string userId)
        {
            return _context.Invitations
                .Where(i => i.UserId == userId)
                .ToList();
        }

        public bool IsUserHavingAnyInvitation(string userId)
        {
            return _context.Invitations
                .Where(i => i.UserId == userId)
                .Any();
        }
    }
}
