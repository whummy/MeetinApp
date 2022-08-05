using Domain.Entities.Identities;
using Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contract.Identities
{
    public interface IUserRoleRepository : IRepositoryBase<UserRole>
    {
    }
}
