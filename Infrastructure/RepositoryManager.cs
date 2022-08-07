using Infrastructure.Contract;
using Infrastructure.Contract.Identities;
using Infrastructure.Contracts;
using Infrastructure.Data.DbContext;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Identities;

namespace Infrastructure;

public class RepositoryManager : IRepositoryManager
{
    private readonly AppDbContext _appDbContext;
    private readonly Lazy<IRoleRepository> _roleRepository;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IUserRoleRepository> _userRoleRepository;
    private readonly Lazy<IMeetingRepository> _meetingRepository;
    private readonly Lazy<IParticipantRepository> _participantRepository;
    // private readonly Lazy<ITokenRepository> _tokenRepository;

    public RepositoryManager(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _roleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(appDbContext));
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(appDbContext));
        _userRoleRepository = new Lazy<IUserRoleRepository>(() => new UserRoleRepository(appDbContext));
        _meetingRepository = new Lazy<IMeetingRepository>(() => new MeetingRepository(appDbContext));
        _participantRepository = new Lazy<IParticipantRepository>(() => new ParticipantRepository(appDbContext));
    }

    public IRoleRepository Role => _roleRepository.Value;
    public IUserRepository User => _userRepository.Value;
    public IUserRoleRepository UserRole => _userRoleRepository.Value;
    public IMeetingRepository Meeting => _meetingRepository.Value;
    public IParticipantRepository Participant => _participantRepository.Value;
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