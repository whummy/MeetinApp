using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataTransferObjects
{
    public class ParticipantDTO
    {
        //public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class ParticipantCreateDTO
    {

    }

    public record ParticipantGetDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
