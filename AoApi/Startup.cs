using System;
using System.Collections.Generic;
using System.Linq;
using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;
using AoApi.Services.PropertyMappingServices;
using AoApi.Services;
using AoApi.Services.Data.DtoModels.EmployeeDtos;
using AoApi.Services.Data.DtoModels.JobDtos;
using AoApi.Services.Data.DtoModels.RoleDtos;
using AoApi.Services.Data.DtoModels.ScheduleDtos;
using AoApi.Services.Data.DtoModels.UserDtos;
using AoApi.Services.Data.DtoModels.WalletDtos;
using AoApi.Services.Data.DtoModels.WorkhoursDtos;
using AoApi.Services.Data.Repositories;
using AoApi.Services.PropertyMappingServices;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            var connectionString = Configuration["ConnectionStrings:AoDB"];
            services.AddDbContext<AOContext>(x => x.UseSqlServer(connectionString, y => y.MigrationsAssembly("AoApi.Data")));
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWorkhoursRepository, WorkhoursRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddSingleton<IPropertyMappingService, PropertyMappingService>(implementationFactory =>
            {
                var pms = new PropertyMappingService();

                pms.AddPropertyMapping<EmployeeDtoForMultiple, Employee>(new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Id", new List<string>() { "Id" }},
                    { "Name", new List<string>() { "FirstName", "LastName" }},
                    { "Address", new List<string>() { "City", "Country", "Street"}}
                });

                return pms;
            });

            
            services.AddScoped<IPaginationUrlHelper, PaginationUrlHelper>();
            services.AddScoped<IHateoasHelper, HateoasHelper>();
            services.AddTransient<ITypeHelperService, TypeHelperService>();
            services.AddScoped<IControllerHelper, ControllerHelper>();

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
                config.CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.JobTitle));
                config.CreateMap<EmployeeCreateDto, Employee>();
                config.CreateMap<EmployeeUpdateDto, Employee>();
                config.CreateMap<Employee, EmployeeUpdateDto>();
                config.CreateMap<Employee, EmployeeDtoForMultiple>()
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Street} {src.City} {src.Country}"))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

                config.CreateMap<Job, JobDto>();
                config.CreateMap<JobCreateDto, Job>();
                config.CreateMap<JobUpdateDto, Job>();
                config.CreateMap<Job, JobUpdateDto>();

                config.CreateMap<Role, RoleDto>();
                config.CreateMap<RoleCreateDto, Role>();
                config.CreateMap<RoleUpdateDto, Role>();
                config.CreateMap<Role, RoleUpdateDto>();

                config.CreateMap<Schedule, ScheduleDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName));
                config.CreateMap<ScheduleCreateDto, Schedule>();
                config.CreateMap<ScheduleUpdateDto, Schedule>();
                config.CreateMap<Schedule, ScheduleUpdateDto>();

                config.CreateMap<User, UserDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName))
                .ForMember(dest => dest.RoleTitle, opt => opt.MapFrom(src => src.Role.RoleTitle));
                config.CreateMap<UserCreateDto, User>();
                config.CreateMap<UserUpdateDto, User>();
                config.CreateMap<User, UserUpdateDto>();

                config.CreateMap<Wallet, WalletDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName));
                config.CreateMap<WalletCreateDto, Wallet>();
                config.CreateMap<WalletUpdateDto, Wallet>();
                config.CreateMap<Wallet, WalletUpdateDto>();

                config.CreateMap<Workhours, WorkhoursDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName));
                config.CreateMap<WorkhoursCreateDto, Workhours>();
                config.CreateMap<WorkhoursUpdateDto, Workhours>();
                config.CreateMap<Workhours, WorkhoursUpdateDto>();
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
