using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OrderClaptrap.BackendServer.EntityFrameworkCore
{
    public class OrderClaptrapBackendServerMigrationsDbContextFactory : IDesignTimeDbContextFactory<OrderClaptrapBackendServerMigrationsDbContext>
    {
        public OrderClaptrapBackendServerMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<OrderClaptrapBackendServerMigrationsDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"), MySqlServerVersion.LatestSupportedServerVersion);

            return new OrderClaptrapBackendServerMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}