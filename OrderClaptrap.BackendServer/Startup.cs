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
using Volo.Abp;

namespace OrderClaptrap.BackendServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ServiceInitial(services, Configuration);
            services.AddApplication<OrderClaptrapBackendModule>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule<ActorsModule>();

            _claptrapBootstrapper.Boot(builder);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.InitializeApplication();
        }

        private void ServiceInitial(IServiceCollection services, IConfiguration configuration)
        {
            services.AddClaptrapServerOptions();
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

            services.AddClaptrapServerOptions();
            services.AddActors(options => { options.AddClaptrapDesign(_claptrapDesignStore); });
            services.AddControllers()
                .AddDapr();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderClaptrap.BackendServer", Version = "v1" });
            });
        }

        private readonly AutofacClaptrapBootstrapper _claptrapBootstrapper;
        private readonly IClaptrapDesignStore _claptrapDesignStore;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var loggerFactory = new ServiceCollection()
                .AddLogging(logging => logging.AddConsole())
                .BuildServiceProvider()
                .GetRequiredService<ILoggerFactory>();

            var bootstrapperBuilder = new AutofacClaptrapBootstrapperBuilder(loggerFactory);
            _claptrapBootstrapper = (AutofacClaptrapBootstrapper)bootstrapperBuilder
                .ScanClaptrapModule()
                .AddConfiguration(configuration)
                .ScanClaptrapDesigns(new[] { typeof(AuctionItemActor).Assembly })
                .UseDaprPubsub(pubsub => pubsub.AsEventCenter())
                .Build();
            _claptrapDesignStore = _claptrapBootstrapper.DumpDesignStore();
        }
    }
}