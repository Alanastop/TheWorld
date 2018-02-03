using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using TheWorld.MVC.Models;
using TheWorld.Models.Persistent;
using TheWorld.Models.Persistent.Interfaces;
using TheWorld.Models;
using Microsoft.AspNetCore.Identity;
using TheWorld.DbContext;
using TheWorld.Repositories.Interfaces;
using TheWorld.Repositories;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TheWorld.MVC
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
            services.AddMvc()
            .AddJsonOptions(config =>
             {
                 config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
             });

            services.AddSingleton(this.Configuration);
            services.AddDbContext<WorldContext>();

            services.AddIdentity<WorldUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
            })
           .AddEntityFrameworkStores<WorldContext>();
           
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/login";
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        await Task.Yield();
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<TripViewModel, Trip>().ReverseMap();
                config.CreateMap<StopViewModel, Stop>().ReverseMap();
            });

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(
              config =>
              {
                  config.MapRoute(
                        name: "Default",
                        template: "{controller}/{action}/{id?}",
                        defaults: new { controller = "TripsMvc", action = "Index" });
              });
        }
    }
}
