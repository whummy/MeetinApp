using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public MeetingService(IRepositoryManager repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<SuccessResponse<MeetingDTO>> CreateMeeting(MeetingCreateDTO model)
        {
            var meetingExist = await _repository.Meeting.Get(x => x.Title.ToLower() == model.Title.ToLower()).FirstOrDefaultAsync();
            Guard.AgainstDuplicate(meetingExist, "Meeting already exist", HttpStatusCode.BadRequest);
            var meeting = _mapper.Map<Meeting>(model);
            meeting.MeetingCode = Int32.Parse(GenerateCode());
            await _repository.Meeting.AddAsync(meeting);
            await _repository.SaveChangesAsync();
            return new SuccessResponse<MeetingDTO>
            {
                Data = _mapper.Map<MeetingDTO>(meeting),
                Message = "Meeting successfully created",
                Success = true
            };
        }

        private string GenerateCode()
        {
            Random rnd = new Random();
            var randomNumber1 = rnd.Next(1000, 10000);
            var randomNumber2 = DateTime.Now.Second;
            return $"{randomNumber1}{randomNumber1}";
        }
    }
}
