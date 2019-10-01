using System.IO;
using SomeBasicEFApp.Web.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace SomeBasicEFApp.Tests
{
    public class SqlLiteCustomerDataTests : CustomerDataTests
    {
        private static Lazy<DbContextOptions> options = new Lazy<DbContextOptions>(() =>
        {
            if (File.Exists("CoreTests.db")) File.Delete("CoreTests.db");
            var opts = new DbContextOptionsBuilder()
                             .UseSqlite("Data Source=CoreTests.db")
                             .Options;
            using (var db = new CoreDbContext(opts))
            {
                db.Database.Migrate();
            }
            return Setup(opts);
        });
        public override DbContextOptions Options => options.Value;
    }
}
