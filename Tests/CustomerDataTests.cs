using System.IO;
using System.Xml.Linq;
using SomeBasicEFApp.Web.Data;
using SomeBasicEFApp.Web.Entities;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using SomeBasicEFApp.Web;

namespace SomeBasicEFApp.Tests
{
    public class CustomerDataTests : IDisposable
    {
        private CoreDbContext _session;
        public CustomerDataTests()
        {
            _session = new CoreDbContext(options.Value);
        }

        [Fact]
        public void CanGetCustomerById()
        {
            var customer = _session.GetCustomer(new CustomerId("1"));

            Assert.NotNull(customer);
        }

        [Fact]
        public void CanGetCustomerByFirstname()
        {
            var customers = _session.Customers
                .Where(c => c.Firstname == "Steve")
                .ToList();
            Assert.Equal(3, customers.Count);
        }

        [Fact]
        public void CanGetProductById()
        {
            var product = _session.GetProduct(new ProductId("1"));

            Assert.NotNull(product);
        }
        [Fact]
        public void OrderContainsProduct()
        {
            var o = _session.Orders
                    .Include(order => order.ProductOrders)
                        .ThenInclude(po => po.Product)
                    .Single(order => order.Id == new OrderId("1"));
            Assert.NotNull(o.ProductOrders);
            Assert.True(o.ProductOrders.Any(p => p.Product.Id == new ProductId("1")));
        }
        [Fact]
        public void OrderHasACustomer()
        {
            var o = _session.Orders
                    .Include(order => order.Customer)
                    .Single(order => order.Id == new OrderId("1"));
            Assert.NotNull(o.Customer);
            Assert.NotEmpty(o.Customer.Firstname);
        }

        private static string rnd => Guid.NewGuid().ToString("N");
        private static Lazy<DbContextOptions<CoreDbContext>> options = new Lazy<DbContextOptions<CoreDbContext>>(() =>
            Setup(new DbContextOptionsBuilder<CoreDbContext>()
                                         .UseInMemoryDatabase(databaseName: $"customer_data_tests_{rnd}").Options));

        private static DbContextOptions<CoreDbContext> Setup(DbContextOptions<CoreDbContext> opts)
        {
            var doc = XDocument.Load(Path.Combine(
                Path.GetDirectoryName(typeof(CustomerDataTests).GetTypeInfo().Assembly.Location),
                "TestData", "TestData.xml"));
            var import = new XmlImport(doc, "http://tempuri.org/Database.xsd");
            var customer = new List<Customer>();
            using (var session = new CoreDbContext(opts))
            {
                import.Parse(new[] { typeof(Customer), typeof(Order), typeof(Product) },
                                (type, obj) =>
                                {
                                    switch (type.Name)
                                    {
                                        case nameof(Customer):
                                            session.Customers.Add((Customer)obj);
                                            break;
                                        case nameof(Order):
                                            session.Orders.Add((Order)obj);
                                            break;
                                        case nameof(Product):
                                            session.Products.Add((Product)obj);
                                            break;
                                        default:
                                            break;
                                    }

                                });
                session.SaveChanges();
            }

            using (var session = new CoreDbContext(opts))
            {
                import.ParseConnections("OrderProduct", "Product", "Order", (productId, orderId) =>
                {
                    session.ProductOrders.Add(new ProductOrder { 
                        OrderId = new OrderId(orderId.ToString()), 
                        ProductId = new ProductId(productId.ToString())
                    });
                });

                import.ParseIntProperty("Order", "Customer", (orderId, customerId) =>
                {
                    var order = session.GetOrder(new OrderId(orderId.ToString()));
                    order.CustomerId = new CustomerId(customerId.ToString());
                });

                session.SaveChanges();
            }
            return opts;
        }

        public void Dispose()
        {
            ((IDisposable)_session).Dispose();
        }
    }
}
