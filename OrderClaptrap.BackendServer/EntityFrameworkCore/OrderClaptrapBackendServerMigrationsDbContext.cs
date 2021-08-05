using Microsoft.EntityFrameworkCore;
using OrderClaptrap.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrderClaptrap.BackendServer.EntityFrameworkCore
{
    [ConnectionStringName(OrderClaptrapDbProperties.ConnectionStringName)]
    public class OrderClaptrapBackendServerMigrationsDbContext : AbpDbContext<OrderClaptrapBackendServerMigrationsDbContext>
    {
        public OrderClaptrapBackendServerMigrationsDbContext(DbContextOptions<OrderClaptrapBackendServerMigrationsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureOrderClaptrap();
        }
    }
}