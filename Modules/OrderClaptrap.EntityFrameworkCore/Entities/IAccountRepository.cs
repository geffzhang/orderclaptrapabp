using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories;

namespace OrderClaptrap.EntityFrameworkCore.Entities
{
    public interface IAccountRepository : IRepository<Account, Guid>
    {
    }
}