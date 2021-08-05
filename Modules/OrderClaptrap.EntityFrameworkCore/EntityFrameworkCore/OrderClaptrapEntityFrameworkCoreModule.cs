using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace OrderClaptrap.EntityFrameworkCore.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class OrderClaptrapEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<OrderClaptrapDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });
        }
    }
}