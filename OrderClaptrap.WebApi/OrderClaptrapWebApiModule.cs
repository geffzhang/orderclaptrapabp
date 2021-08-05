using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newbe.Claptrap;
using OpenTelemetry.Trace;
using OrderClaptrap.EntityFrameworkCore.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;

namespace OrderClaptrap.WebApi
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpHttpClientIdentityModelWebModule),
        typeof(OrderClaptrapEntityFrameworkCoreModule)
        )]
    public class OrderClaptrapWebApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ServiceInitial(context.Services, context.Services.GetConfiguration());
            Configure<AbpAntiForgeryOptions>(options =>
            {
                options.AutoValidate = false;
            });
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
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderClaptrap.WebApi v1"));
            //}
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAbpClaimsMap();

            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                var configuration = context.GetConfiguration();
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
                options.OAuthScopes("EasyDemo");
            });
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseCors(builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(x => true));
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Map("/", c =>
                {
                    c.Response.Redirect("/swagger");
                    return Task.CompletedTask;
                });
            });
        }

        private void ServiceInitial(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenTelemetryTracing(
                 builder => builder
                     .AddSource(ClaptrapActivitySource.Instance.Name)
                     .SetSampler(new ParentBasedSampler(new AlwaysOnSampler()))
                     .AddAspNetCoreInstrumentation()
                     .AddGrpcClientInstrumentation()
                     .AddHttpClientInstrumentation()
                     .AddZipkinExporter(options =>
                     {
                         var zipkinBaseUri = configuration.GetServiceUri("zipkin", "http");
                         options.Endpoint = new Uri(zipkinBaseUri!, "api/v2/spans");
                     })
             );

            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            services
                .AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "EasyDemo-Protection-Keys");
            Console.WriteLine(configuration.GetServiceUri("zipkin", "http").AbsoluteUri);
            services.AddControllers();
            services.AddActors(_ => { });

            services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"],
                new Dictionary<string, string>
                {
                    {"EasyDemo", "EasyDemo API"},
                    {"IdentityService", "Identity Service API"}
                },
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "EasyDemo API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "EasyDemo";
                    options.TokenValidationParameters.ValidateIssuer = false;
                });

            services.AddSameSiteCookiePolicy();
        }
    }
}