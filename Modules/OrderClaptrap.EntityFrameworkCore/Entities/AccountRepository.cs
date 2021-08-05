using OrderClaptrap.EntityFrameworkCore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OrderClaptrap.EntityFrameworkCore.Entities
{
    public class AccountRepository : EfCoreRepository<IOrderClaptrapDbContext, Account, Guid>, IAccountRepository
    {
        public AccountRepository(IDbContextProvider<IOrderClaptrapDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}