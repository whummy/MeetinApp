using Infrastructure.Contract;
using Infrastructure.Contract.Identities;

namespace Infrastructure.Contracts;

public interface IRepositoryManager
{
    IRoleRepository Role { get; }
    //IUserActivityRepository UserActivity { get; }
    IUserRepository User { get; }
    IMeetingRepository Meeting { get; }
    IUserRoleRepository UserRole { get; }
    IParticipantRepository Participant { get; }
    //ITokenRepository Token { get; }
    Task BeginTransaction(Func<Task> action);
    Task SaveChangesAsync();
}
