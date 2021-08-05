using System;
using Autofac;
using OrderClaptrap.Actors;
using OrderClaptrap.Actors.AuctionItem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newbe.Claptrap;
using Newbe.Claptrap.Bootstrapper;
using OpenTelemetry.Trace;
using Newbe.Claptrap.StorageProvider.MySql;
using Volo.Abp.Modularity;
using OrderClaptrap.EntityFrameworkCore.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Swashbuckle;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Threading;
using Volo.Abp.Data;

namespace OrderClaptrap.BackendServer
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(OrderClaptrapEntityFrameworkCoreModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class OrderClaptrapBackendModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
            });
        }

        public override void OnApplicationInitialization(
            ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderClaptrap.BackendServer v1"));

            app.UseRouting();

            app.UseCloudEvents();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapActorsHandlers();
                endpoints.MapSubscribeHandler();
                endpoints.MapControllers();
            });
            SeedData(context);
        }

        private void SeedData(ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(async () =>
            {
                using (var scope = context.ServiceProvider.CreateScope())
                {
                    await scope.ServiceProvider
                        .GetRequiredService<IDataSeeder>()
                        .SeedAsync();
                }
            });
        }
    }
}