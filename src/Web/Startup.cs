using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SomeBasicEFApp.Web.Data;

namespace SomeBasicEFApp.Web;

///
public class Startup
{
    private readonly SwaggerConfig _swagger;

    ///
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        _swagger = new SwaggerConfig();
    }
    ///
    public IConfiguration Configuration { get; }
    ///
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add framework services.
        services.AddDbContext<CoreDbContext>(ConfigureDbContext);

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
        services.AddControllersWithViews().AddApplicationPart(typeof(Startup).Assembly);
    }

    protected virtual void ConfigureDbContext(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
    }

    ///
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
        OnConfigured(app, env);
    }

    protected virtual void OnConfigured(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}
