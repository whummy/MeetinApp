using Infrastructure.Contract.Identities;

namespace Infrastructure.Contracts;

public interface IRepositoryManager
{
    IRoleRepository Role { get; }
    //IUserActivityRepository UserActivity { get; }
    IUserRepository User { get; }
    IUserRoleRepository UserRole { get; }
    //ITokenRepository Token { get; }
    Task BeginTransaction(Func<Task> action);
    Task SaveChangesAsync();
}
