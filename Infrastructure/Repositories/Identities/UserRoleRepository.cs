using Domain.Entities.Identities;
using Infrastructure.Contract.Identities;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Repositories.Identities
{
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AppDbContext context) : base(context)
        {
        }
    }
}
