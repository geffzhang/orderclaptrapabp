using Microsoft.EntityFrameworkCore;
using OrderClaptrap.EntityFrameworkCore.Entities;
using OrderClaptrap.EntityFrameworkCore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OrderClaptrap.EntityFrameworkCore
{
    public static class OrderClaptrapDbContextModelCreatingExtensions
    {
        public static void ConfigureOrderClaptrap(
            this ModelBuilder builder,
            Action<OrderClaptrapModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new OrderClaptrapModelBuilderConfigurationOptions(
                OrderClaptrapDbProperties.DbTablePrefix,
                OrderClaptrapDbProperties.DbSchema
            );
            optionsAction?.Invoke(options);
            builder.Entity<Account>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Account), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<Product>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Product), options.Schema);
                b.ConfigureByConvention();
            });
        }
    }
}