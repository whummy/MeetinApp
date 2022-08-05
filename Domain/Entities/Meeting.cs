﻿using Domain.Entities.Identities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Meeting
    {
        public Guid Id { get; set; }
        public int MeetingCode { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
       // public Guid? CreatedById { get; set; }
       // public User Creator { get; set; }
        public ICollection<User> Participants { get; set; }
    }
}
