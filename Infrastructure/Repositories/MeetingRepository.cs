using Domain.Entities;
using Infrastructure.Contract;
using Infrastructure.Contracts;
using Infrastructure.Data.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MeetingRepository : RepositoryBase<Meeting>, IMeetingRepository
    {
        public MeetingRepository(AppDbContext context) : base(context)
        {
        }

    }
}
