﻿using Application.Contracts;
using AutoMapper;
using Domain.Entities.Identities;
using Infrastructure.Contracts;
using Infrastructure.Utils.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IMeetingService> _meetingService;


    public ServiceManager(IRepositoryManager repositoryManager,
        ILoggerManager logger,
        IMapper mapper,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration)
    {
        _authenticationService =
            new Lazy<IAuthenticationService>(
                () => new AuthenticationService(repositoryManager, userManager, mapper, logger, configuration));

        _userService = new Lazy<IUserService>(
                () => new UserService(repositoryManager, userManager, roleManager, mapper, logger, configuration));

        _meetingService = new Lazy<IMeetingService>(
                () => new MeetingService(repositoryManager, mapper));
    }

    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public IUserService UserService => _userService.Value;
    public IMeetingService MeetingService => _meetingService.Value;
}