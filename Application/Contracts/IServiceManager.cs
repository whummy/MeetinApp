namespace Application.Contracts;

public interface IServiceManager
{
    IAuthenticationService AuthenticationService { get; }
    IUserService UserService { get; }
    IMeetingService MeetingService { get; }
    IParticipantService ParticipantService { get; }
}
