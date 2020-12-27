using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tranzact.SearchFight.API.Entities;
using Tranzact.SearchFight.Domain.Interface;
using Tranzact.SearchFight.Domain.SearchEngine;
using Tranzact.SearchFight.Transversal;

namespace Tranzact.SearchFight.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IMapper iMapper = Maps.InitMapper();
            services.AddSingleton(iMapper);

            services.AddOptions();
            services.Configure<GoogleEngine>(Configuration.GetSection("GoogleEngine"));
            services.Configure<MSNEngine>(Configuration.GetSection("MSNEngine"));

            services.AddTransient<InterfaceFactorySearchEngine, FactorySearchengine>();
            services.AddHttpClient<InterfaceSearchEngineDomain, MSNSearchEngineDomain>();
            services.AddHttpClient<InterfaceSearchEngineDomain, GoogleSearchEngineDomain>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
