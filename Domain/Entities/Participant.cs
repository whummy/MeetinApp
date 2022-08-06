using Domain.Entities.Identities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Participant
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}
//repo.Meeting.