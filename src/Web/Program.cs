using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SomeBasicEFApp.Web
{
    public class Program
    {
        public static void Main(string[] args) => BuildWebHost(args).Run();

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, conf) =>
                {
                    conf.AddJsonFile("appsettings.json", reloadOnChange: true, optional: true)
                        .AddJsonFile("appsettings.user.json", reloadOnChange: true, optional: true)
                        .AddEnvironmentVariables();
                    if (args != null)
                    {
                        conf.AddCommandLine(args);
                    }
                })
                .ConfigureLogging((hostingContext, logging) => { logging.AddConsole().AddDebug(); })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                })
#if DEBUG
                .CaptureStartupErrors(true)
#endif
                .Build();
    }
}
