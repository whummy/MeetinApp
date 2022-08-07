﻿using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class MeetingMapper : Profile
    {
        public MeetingMapper()
        {
            CreateMap<MeetingCreateDTO, Meeting>();
            CreateMap<Meeting, MeetingDTO>();
        }
    }
}
