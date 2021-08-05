using Microsoft.EntityFrameworkCore;
using OrderClaptrap.EntityFrameworkCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EntityFrameworkCore;

namespace OrderClaptrap.EntityFrameworkCore.EntityFrameworkCore
{
    public interface IOrderClaptrapDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
        DbSet<Account> Accounts { get; }
        DbSet<Product> Products { get; }
    }
}