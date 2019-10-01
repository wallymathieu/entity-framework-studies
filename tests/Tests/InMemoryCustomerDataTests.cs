using Microsoft.EntityFrameworkCore;
using System;

namespace SomeBasicEFApp.Tests
{
    public class InMemoryCustomerDataTests : CustomerDataTests
    {
        private static string rnd => Guid.NewGuid().ToString("N");
        private static Lazy<DbContextOptions> options = new Lazy<DbContextOptions>(() =>
              Setup(new DbContextOptionsBuilder()
                  .UseInMemoryDatabase(databaseName: $"customer_data_tests_{rnd}")
                  .Options));
        public override DbContextOptions Options => options.Value;
    }
}
