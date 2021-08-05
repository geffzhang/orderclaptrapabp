using OrderClaptrap.EntityFrameworkCore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OrderClaptrap.EntityFrameworkCore.Entities
{
    public class ProductRepository : EfCoreRepository<IOrderClaptrapDbContext, Product, Guid>, IProductRepository
    {
        public ProductRepository(IDbContextProvider<IOrderClaptrapDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}