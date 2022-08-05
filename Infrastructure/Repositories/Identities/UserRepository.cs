using Domain.Entities.Identities;
using Infrastructure.Contract.Identities;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Repositories.Identities
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
