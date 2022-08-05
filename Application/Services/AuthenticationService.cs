using Application.Contracts;
using AutoMapper;
using Domain.ConfigurationModels;
using Domain.Entities.Identities;
using Infrastructure.Contracts;
using Infrastructure.Utils.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepositoryManager _repository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly JwtConfiguration _jwtConfiguration;

        public AuthenticationService(IRepositoryManager repository, UserManager<User> userManager,
            IMapper mapper, ILoggerManager logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _jwtConfiguration = new JwtConfiguration();
            _configuration.Bind(_jwtConfiguration.Section, _jwtConfiguration);
        }
    }
}
