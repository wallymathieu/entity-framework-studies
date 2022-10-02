using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Web;

internal class SwaggerConfig
{
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
            var types = new[] {typeof(CustomerId), typeof(OrderId), typeof(ProductId)};
            foreach (var type in types)
            {
                options.MapType(type,() => new OpenApiSchema {
                    Type = "string",
                    Example = new OpenApiString(Activator.CreateInstance(type, 1).ToString() )});
            }

        });
    }
}
