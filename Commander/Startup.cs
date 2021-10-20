using Commander.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Commander.Controllers;

namespace Commander
{
    public class Startup
    {
        //gives access to configuration API
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //Here we add the services that we need
        public void ConfigureServices(IServiceCollection services)
        {

            // needed to use NewtonsoftJson package
            services.AddControllers().AddNewtonsoftJson(s=> {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddScoped<ICommanderRepo, SqlCommanderRepo>();
            services.AddScoped<IServices, Services>();

            //configure db class to be used in the rest of the app
            services.AddDbContext<CommanderContext>(opt => opt.UseSqlServer
            (Configuration.GetConnectionString("CommanderConnection")));

            // make automapper available
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddCors(x => x.AddPolicy("CorsPolicy", builder => builder.AllowCredentials().AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Setup request pipelines
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader().AllowCredentials());

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
