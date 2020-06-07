module WebFs.Swagger
open System
open Microsoft.OpenApi.Models
open Swashbuckle.AspNetCore.Swagger
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
///
type SwaggerConfig()=
    ///
    member __.Configure(app:IApplicationBuilder)=
        // Enable middleware to serve generated Swagger as a JSON endpoint
        app.UseSwagger(fun c ->  c.RouteTemplate <- "swagger/{documentName}/swagger.json" ) |> ignore

        app.UseSwaggerUI(fun c ->
        
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            c.EnableDeepLinking();
        ) |> ignore
    ///
    member __.ConfigureServices(services:IServiceCollection)=
        services.AddSwaggerGen(fun options -> ()) |> ignore

        services.ConfigureSwaggerGen(fun options ->
            let info = OpenApiInfo(
                        Version = "dev",
                        Title = "API",
                        Description = "Some API",
                        Contact=OpenApiContact(
                                    Name = "Dev",
                                    Email = "developers@somecompany.com",
                                    Url = Uri("https://somecompany.com")))

            options.SwaggerDoc("v1", info);
        ) |> ignore
