using MyForum.Core;
using MyForum.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyForum.Data.Services
{
    public class InvitationData : IInvitationData
    {
        private readonly MyForumDbContext db;

        public InvitationData(MyForumDbContext db)
        {
            this.db = db;
        }

        public Invitation Add(Invitation newInvitation)
        {
            db.Add(newInvitation);
            return newInvitation;
        }

        public Invitation Delete(Invitation invitationToDelete)
        {
            db.Invitations.Remove(invitationToDelete);
            return invitationToDelete;
        }

        public Invitation Delete(string userId, int guildId)
        {
            var invitation = Find(userId, guildId);
            if(invitation != null)
            {
                db.Invitations.Remove(invitation);
            }
            
            return invitation;
        }

        public Invitation Find(string userId, int guildId)
        {
            return db.Invitations.Find(new { UserId = userId, GuildId = guildId });
        }

        public IEnumerable<Invitation> GetByGuildId(int guildId)
        {
            var query = db.Invitations
                .Where(i => i.GuildId == guildId)
                .ToList();

            return query;
        }

        public IEnumerable<Invitation> GetByUserId(string userId)
        {
            var query = db.Invitations
                .Where(i => i.UserId == userId)
                .ToList();

            return query;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }
    }
}
