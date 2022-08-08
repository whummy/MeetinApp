using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly UserManager<User> _userManager;

        public ParticipantService(IRepositoryManager repository, IMapper mapper, IHttpContextAccessor httpAccessor, UserManager<User> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _httpAccessor = httpAccessor;
            _userManager = userManager;
        }

        public async Task<Helpers.SuccessResponse<string>> JoinMeeting(int meetingCode, LoggedinUserDto loggedinUser)
        {
            //check if meeting with that code exists and active
            var meeting = await _repository.Meeting.Get(x => x.MeetingCode == meetingCode && x.IsActive == true).FirstOrDefaultAsync();
            Guard.AgainstNull(meeting, HttpStatusCode.BadRequest);

            var userId = loggedinUser.UserId;

            var IsInMeeting = await _repository.Participant.Get(x => x.UserId == userId && x.MeetingId == meeting.Id).FirstOrDefaultAsync();
            Guard.AgainstDuplicate(IsInMeeting, "You are already in the meeting");

            // we want a participant equivalent to the user with id userId

            var newParticipant = new Participant { UserId = userId, MeetingId = meeting.Id };

            await _repository.Participant.AddAsync(newParticipant);

            _repository.SaveChangesAsync();

            return new SuccessResponse<string>
            {
                Success = true,
                Data = null,
                Message = "Successfuly joined"
            };

        }

        
    }
}
