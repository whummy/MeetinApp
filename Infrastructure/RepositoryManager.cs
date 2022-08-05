using Infrastructure.Contracts;
using Infrastructure.Contracts.Identities;
using Infrastructure.Contracts.CampaignPoll;
using Infrastructure.Data.DbContext;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Identities;
using Infrastructure.Repositories.CampaignPoll;
using Infrastructure.Contract.Identities;

namespace Infrastructure;

public class RepositoryManager : IRepositoryManager
{
    private readonly AppDbContext _appDbContext;
    private readonly Lazy<IRoleRepository> _roleRepository;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IUserRoleRepository> _userRoleRepository;
   // private readonly Lazy<IUserActivityRepository> _userActivityRepository;
   // private readonly Lazy<ITokenRepository> _tokenRepository;

    public RepositoryManager(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _roleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(appDbContext));
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(appDbContext));
        _userRoleRepository = new Lazy<IUserRoleRepository>(() => new UserRoleRepository(appDbContext));
       // _userActivityRepository = new Lazy<IUserActivityRepository>(() => new UserActivityRepository(appDbContext));
       // _tokenRepository = new Lazy<ITokenRepository>(() => new TokenRepository(appDbContext));
    }

    public IRoleRepository Role => _roleRepository.Value;
    public IUserRepository User => _userRepository.Value;
    public IUserRoleRepository UserRole => _userRoleRepository.Value;
   // public ITokenRepository Token => _tokenRepository.Value;
   // public IUserActivityRepository UserActivity => _userActivityRepository.Value;

    public async Task SaveChangesAsync() => await _appDbContext.SaveChangesAsync();
    public async Task BeginTransaction(Func<Task> action)
    {
        await using var transaction = await _appDbContext.Database.BeginTransactionAsync();
        try
        {
            await action();

            await SaveChangesAsync();
            await transaction.CommitAsync();

        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}