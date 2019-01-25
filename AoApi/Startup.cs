﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoApi.Data;
using AoApi.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace AoApi
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
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                var jsonOutputFormatter = setupAction.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    jsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.AO.json+hateoas");
                }

            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            var connectionString = Configuration["ConnectionStrings:AoDB"];
            services.AddDbContext<AOContext>(x => x.UseSqlServer(connectionString, y => y.MigrationsAssembly("AoApi.Data")));
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new Info { Title = "Administrative Organizer Api", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            Mapper.Initialize(config =>
            {

            });

            app.UseSwaggerUI(c =>
            {
                c.EnableDeepLinking();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Administrative Organizer Api v1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
