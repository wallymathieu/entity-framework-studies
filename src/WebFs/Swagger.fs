module WebFs.Swagger
open Swashbuckle.AspNetCore.Swagger
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
///
type SwaggerConfig()=
    ///
    member __.Configure(app:IApplicationBuilder, env:IHostingEnvironment)=
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
            let info = Info()
            info.Version <- "dev"
            info.Title <- "API"
            info.Description <- "Some API"
            info.TermsOfService <- "See license agreement"
            let contact=Contact()
            contact.Name <- "Dev"
            contact.Email <- "developers@somecompany.com"
            contact.Url <- "https://somecompany.com"
            info.Contact <- contact 
            options.SwaggerDoc("v1", info);
        ) |> ignore
