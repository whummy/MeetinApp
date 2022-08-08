using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<PagedResponse<IEnumerable<ParticipantGetDTO>>> GetParticipantsByMeeting(Guid meetingId, ResourceParameter parameter, string name, IUrlHelper urlHelper)
        {
            var meeting = await _repository.Meeting.Get(x => x.Id == meetingId).FirstOrDefaultAsync();
            Guard.AgainstNull(meeting);

            IQueryable<Participant> query;
            query = _repository.Participant.QueryAll()
                        .Include(x => x.User)
                        .Where(x => x.MeetingId == meetingId);

            var agentsQuery = query.Select(x => new ParticipantGetDTO
            {
                Id = x.UserId,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                Email = x.User.Email,
                PhoneNumber = x.User.PhoneNumber,
            });

            if (!string.IsNullOrWhiteSpace(parameter.Search))
            {
                var search = parameter.Search.Trim().ToLower();
                agentsQuery = agentsQuery.Where(
                    a => a.FirstName.ToLower().Contains(search) || a.LastName.ToLower().Contains(search)
                        || a.Email.ToLower().Contains(search));
            };

            var pagedParticipantsDto = await PagedList<ParticipantGetDTO>.CreateAsync(agentsQuery, parameter.PageNumber, parameter.PageSize, parameter.Sort);
            var page = PageUtility<ParticipantGetDTO>.CreateResourcePageUrl(parameter, name, pagedParticipantsDto, urlHelper);

            var response = new PagedResponse<IEnumerable<ParticipantGetDTO>>
            {
                Message = "Data retrieved successfully",
                Data = pagedParticipantsDto,
                Meta = new Meta
                {
                    Pagination = page
                }
            };
            return response;

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
