using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SomeBasicEFApp.Web.Data;
using Swashbuckle.AspNetCore.Swagger;

namespace SomeBasicEFApp.Web
{
    public class Startup
    {
        private SwaggerConfig _swagger;
        private IWebHostEnvironment _env;

        class SwaggerConfig
        {
            private IWebHostEnvironment env;

            public SwaggerConfig(IWebHostEnvironment env)
            {
                this.env = env;
            }

            ///
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
                    c.EnableDeepLinking();
                });
            }

            ///
            public virtual void ConfigureServices(IServiceCollection services)
            {
                services.AddSwaggerGen(options => { });

                services.ConfigureSwaggerGen(options =>
                {
                    var webAssembly = typeof(Startup).GetTypeInfo().Assembly;
                    var informationalVersion =
                        (webAssembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute))
                            as AssemblyInformationalVersionAttribute[])?.First()?.InformationalVersion;

                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = informationalVersion ?? "dev",
                        Title = "API",
                        Description = "Some API",
                        Contact = new OpenApiContact
                            {Name = "Dev", Email = "developers@somecompany.com", Url = new Uri("https://somecompany.com")}
                    });

                    //Set the comments path for the swagger json and ui.
                    var xmlPath = typeof(Startup).Assembly.Location.Replace(".dll",".xml").Replace(".exe", ".xml");
                    if (File.Exists(xmlPath))
                        options.IncludeXmlComments(xmlPath);
                });
            }
        }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            _swagger = new SwaggerConfig(env);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<CoreDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            _swagger.ConfigureServices(services);
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
#if DEBUG
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
#endif
                }).AddEntityFrameworkStores<CoreDbContext>()
                .AddDefaultTokenProviders();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler("/Home/Error");
            }

            _swagger.Configure(app, env);
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
