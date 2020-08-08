using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grand.Api.Queries.Handlers.Common;
using Grand.Core.Caching;
using Grand.Core.Data;
using Grand.Framework.Infrastructure.Extensions;
using Grand.Services.Customers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Autofac;
using Microsoft.OpenApi.Models;
using Autofac.Core;
using Swashbuckle.Swagger;

namespace AmicusAPI
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //   Configuration = configuration;
        //}

        public Startup(IHostEnvironment environment)
        {
            //create configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("App_Data/appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddControllers();
            services.ConfigureApplicationServices(Configuration);

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("V1", new OpenApiInfo
            //    {
            //      Version = "V1",
            //      Title = "MyAPI",
            //      Description = "Testing"
            //    });
            //});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new  OpenApiInfo { Version = "V1", Title = "MyAPI",  Description = "Testing" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureRequestPipeline(env);
        
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1"); //originally "./swagger/v1/swagger.json"
            });

            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI");
            //});



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

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigureContainer(Configuration);
        }
    }
}
