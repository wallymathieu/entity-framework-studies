using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using SomeBasicEFApp.Web.Data;
using Swashbuckle.AspNetCore.Swagger;
namespace SomeBasicEFApp.Web
{
    public class Startup
    {
        private SwaggerConfig _swagger;

        class SwaggerConfig
        {
            ///
            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

                    options.SwaggerDoc("v1", new Info
                    {
                        Version = informationalVersion ?? "dev",
                        Title = "API",
                        Description = "Some API",
                        TermsOfService = "See license agreement",
                        Contact = new Contact
                            {Name = "Dev", Email = "developers@somecompany.com", Url = "https://somecompany.com"}
                    });


                    //Determine base path for the application.
                    var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                    //Set the comments path for the swagger json and ui.
                    var xmlPath = Path.Combine(basePath, typeof(Startup).Assembly.GetName().Name + ".xml");
                    if (File.Exists(xmlPath))
                        options.IncludeXmlComments(xmlPath);
                });
            }
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _swagger = new SwaggerConfig();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            
            services.AddDbContext<CoreDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    .ReplaceService<IRelationalTypeMappingSource, IdTypeRelationalTypeMappingSource>()
                    );
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<CoreDbContext>();

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
            services.AddMvc();

            services.AddSwaggerGen((c) => {
                c.SwaggerDoc("v1", new Info { 
                    Version="v1",
                    Title="Current"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            _swagger.Configure(app, env);
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI((c) => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
