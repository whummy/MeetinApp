using Application.Contracts;
using AutoMapper;
using Infrastructure.Contracts;

namespace Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public ParticipantService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
