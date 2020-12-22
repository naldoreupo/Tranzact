using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            services
               .Configure<AppSettings>(Configuration)
               .AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);


            IMapper iMapper = Maps.InitMapper();
            services.AddSingleton(iMapper);

            services.AddTransient<InterfaceFactorySearchEngine, FactorySearchengine>();
            services.AddTransient<InterfaceSearchEngineDomain, GoogleSearchEngineDomain>();
            services.AddTransient<InterfaceSearchEngineDomain, MSNEngineDomain>();
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
