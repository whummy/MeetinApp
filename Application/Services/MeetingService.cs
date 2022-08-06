using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using AutoMapper;
using Infrastructure.Contracts;

namespace Application.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public MeetingService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<MeetingDTO>> CreateMeeting(MeetingCreateDTO model, Guid Id)
        {
            var meeting = await _repository.Meeting.GetByIdAsync(Id);
            if()
        }
    }
}
