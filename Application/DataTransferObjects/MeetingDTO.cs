using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataTransferObjects
{
    public class MeetingDTO : MeetingCreateDTO
    {
        public Guid Id { get; set; }
    }

    public class MeetingCreateDTO
    {
        public int MeetingCode { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }

    }
}
