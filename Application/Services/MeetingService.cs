using Application.Contracts;
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
    }
}
