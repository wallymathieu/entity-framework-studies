using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SomeBasicEFApp.Web;
using SomeBasicEFApp.Web.Data;

namespace SomeBasicEFApp.Tests
{
    public class ApiFixture:IDisposable
    {
        static TestServer Create()
        {
            return new TestServer(new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseConfiguration(new ConfigurationBuilder().Build())
                    .UseStartup<TestStartup>()) { AllowSynchronousIO=true };
        }
        private readonly TestServer _testServer;
        public ApiFixture() => _testServer = Create();
        public void Dispose() 
        {
            _testServer.Dispose();
            if (!File.Exists(db)) return;
            try{ File.Delete(db); }
            catch
            {
                // ignored
            }
        }
        public TestServer Server=>_testServer;

        const string db = "ApiFixture.db";
        class TestStartup : Startup
        {
            public TestStartup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env)
            {
            }
            protected override void ConfigureDbContext(DbContextOptionsBuilder options)
            {
                options.UseSqlite("Data Source=" + db);
            }
            protected override void OnConfigured(IApplicationBuilder app, IWebHostEnvironment env)
            {
                using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = serviceScope.ServiceProvider.GetRequiredService<CoreDbContext>();
                context.Database.Migrate();
            }
        }
    }

}
