// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="">
//   
// </copyright>
// <summary>
//   The startup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorldApi
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authentication.Cookies;
    #region

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json.Serialization;
    using System;
    using TheWorld.DbContext;
    using TheWorld.Models;
    using TheWorld.Models.Persistent;
    using TheWorld.Models.Persistent.Interfaces;
    using TheWorld.MVC.Models;
    using TheWorld.Repositories;
    using TheWorld.Repositories.Interfaces;
    using TheWorld.Services;
    using TheWorldApi.Services;

    #endregion

    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The env local.
        /// </summary>
        private readonly IHostingEnvironment envLocal;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">
        /// The env.
        /// </param>
        public Startup(IHostingEnvironment env)
        {
            this.envLocal = env;
            var builder =
                new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                    .AddJsonFile("config.json")
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        /// <param name="env">
        /// The env.
        /// </param>
        /// <param name="loggerFactory">
        /// The logger factory.
        /// </param>
        /// <param name="seeder">
        /// The seeder.
        /// </param>
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            WorldContextSeedData seeder)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<TripViewModel, Trip>().ReverseMap();
                config.CreateMap<StopViewModel, Stop>().ReverseMap();
            });       

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
                loggerFactory.AddDebug(LogLevel.Information);
            }
            else
            {
                loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
                loggerFactory.AddDebug(LogLevel.Error);
            }

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();
            app.UseAuthentication();
           
            app.UseMvc(
                config =>
                    {
                        config.MapRoute(
                            name: "Default",
                            template: "{controller}/{action}/{id?}",
                            defaults: new { controller = "Trips", action = "Get" });
                    });

            seeder.EnsureSeedData().Wait();
        }

        // This method gets called by the runtime. Use this method to add services to the container.

        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc().AddJsonOptions(
                config =>
                    {
                        config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });
            services.AddLogging();

            services.AddIdentity<WorldUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;                
            })
           .AddEntityFrameworkStores<WorldContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/login";
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .WithExposedHeaders("content-disposition")
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(3600)));
            });
            services.AddSingleton(this.Configuration);
            services.AddDbContext<WorldContext>();
            services.AddTransient<WorldContextSeedData>();
            services.AddScoped<IEntityRepository<Trip>, TripRepository>();
            services.AddScoped<IEntity, Trip>();
            services.AddScoped<IEntityRepository<Stop>, StopRepository>();
            services.AddScoped<IEntity, Stop>();
            services.AddTransient<GeoCoordsService>();

            if (this.envLocal.IsDevelopment())
            {
                services.AddScoped<IMailService, DebugServiceMail>();
            }
            else
            {
                // Implement a diferent service  
            }
        }
    }
}