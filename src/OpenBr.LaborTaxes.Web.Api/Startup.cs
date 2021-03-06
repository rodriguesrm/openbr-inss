using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBr.LaborTaxes.Business.Infra.IoC;
using OpenBr.LaborTaxes.Business.Infra.MongoDb;
using OpenBr.LaborTaxes.Web.Api.Extensions;
using OpenBr.LaborTaxes.Web.Api.Filters;
using RSoft.Logs.Extensions;
using RSoft.Logs.Middleware;
using System;
using System.Text.Json.Serialization;

namespace OpenBr.LaborTaxes.Web.Api
{

    /// <summary>
    /// Application startup object
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// Creates a new instance of the application
        /// </summary>
        /// <param name="configuration">Configuration object</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Injected settings property
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            
            services
                .AddControllersWithViews()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddApplicationSwagger();
            services.AddApplicationService(Configuration);
            services.AddApplicationHealthChecks(Configuration);
            services.AddMiddlewareLoggingOption(Configuration);

            services
                .AddControllers(opt => GlobalFilters.Configure(opt))
                .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder object</param>
        /// <param name="env">Web host environment data</param>
        /// <param name="serviceProvider">Service provider collection</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseApplicationSwagger();
            app.UseApplicationHealthChecks();

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseStaticFiles();
            app.UseResponseCaching();

            app.UseMiddleware<RequestResponseLogging<Startup>>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Create database objects
            IDbDocumentCollectionCreator creators = serviceProvider.GetService<IDbDocumentCollectionCreator>();
            creators.Create().Wait();

            // Seed data from seeders
            var dbDataSeeder = serviceProvider.GetService<IDbDataSeeder>();
            dbDataSeeder.Seed().Wait();

        }
    }
}
