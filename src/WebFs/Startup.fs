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
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.DependencyInjection
open Swagger
open Domain

type Startup private () =
    let swagger = SwaggerConfig()
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1) |> ignore
        services.AddDbContext<CoreDbContext>(fun options ->
                                                 options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")) |> ignore
                                            ) 
                .AddScoped<ICoreDbContext,CoreDbContext>()
                |> swagger.ConfigureServices
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
        else
            app.UseHsts() |> ignore

        app.UseHttpsRedirection() 
            .UseMvc() 
            |> swagger.Configure

    member val Configuration : IConfiguration = null with get, set