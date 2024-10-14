using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Invitation
    {
        public Guid InvitationId { get; set; } = Guid.NewGuid();
        public Guid RestaurantId { get; set; }
        public Guid InvitedUserId { get; set; }
        public Guid InvitingUserId { get; set; }
        public DateTime InvitationDate { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser InvitedUser { get; set; }
        public virtual ApplicationUser InvitingUser { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
