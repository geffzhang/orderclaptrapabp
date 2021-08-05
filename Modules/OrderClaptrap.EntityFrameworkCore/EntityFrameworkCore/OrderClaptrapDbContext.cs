using Microsoft.EntityFrameworkCore;
using OrderClaptrap.EntityFrameworkCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrderClaptrap.EntityFrameworkCore.EntityFrameworkCore
{
    [ConnectionStringName(OrderClaptrapDbProperties.ConnectionStringName)]
    public class OrderClaptrapDbContext : AbpDbContext<OrderClaptrapDbContext>, IOrderClaptrapDbContext
    {
        public OrderClaptrapDbContext(DbContextOptions<OrderClaptrapDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; }

        public DbSet<Product> Products { get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureOrderClaptrap();
        }
    }
}