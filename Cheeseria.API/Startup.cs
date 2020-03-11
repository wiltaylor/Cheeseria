using System;
using System.IO;
using Cheeseria.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Cheeseria.API
{
    /// <summary>
    /// Startup class - Standard ASP.NET Core startup code goes here.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Standard ASP.NET Core configuration object. Pull config from appsettings.json
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Standard ASP.NET Service Dependency Injector
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            //Enable Cors to allow access from any origin. Useful for debugging angular from a different domain in this example.
            services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader()));


            services.AddDbContext<CheeseContext>(options => options.UseInMemoryDatabase("CheeseriaDB"));
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Cheeseria API", Version = "0.1.0"});
                var filePath = Path.Combine(AppContext.BaseDirectory, "Cheeseria.API.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        /// <summary>
        /// Standard ASP.NET Configuration Setup method. Handles all routing and middleware initial configuration.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Setting up Swagger to be located at https://host/swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cheeseria API"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Allow static files to be served out of the root (which will be our angular front end in production).
            app.UseFileServer();

            //Standard ASP.NET setup.
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
