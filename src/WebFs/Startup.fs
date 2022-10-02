namespace WebFs

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy;
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.DependencyInjection
open Swagger
open Domain

type Startup private () =
    let swagger = SwaggerConfig()
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration
    abstract member ConfigureDbContext: DbContextOptionsBuilder ->unit
    default this.ConfigureDbContext(options) =
      options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")) |> ignore
    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.AddControllersWithViews().AddApplicationPart(typeof<Startup>.Assembly) |> ignore

        services.AddDbContext<CoreDbContext>(this.ConfigureDbContext)
                .AddScoped<ICoreDbContext,CoreDbContext>()
                |> swagger.ConfigureServices
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if env.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore
        else
            app.UseHsts() |> ignore

        app.UseHttpsRedirection()
            .UseRouting()
            .UseEndpoints(fun endpoints ->
                endpoints.MapControllerRoute(
                    name= "default",
                    pattern= "{controller=Home}/{action=Index}/{id?}") |> ignore
            )
            |> swagger.Configure

    member val Configuration : IConfiguration = null with get, set
