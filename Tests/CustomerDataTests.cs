using System.IO;
using System.Xml.Linq;
using SomeBasicEFApp.Web.Data;
using SomeBasicEFApp.Web.Entities;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;

namespace SomeBasicEFApp.Tests
{
    public class CustomerDataTests:IDisposable
    {

        private CoreDbContext _session;
        public CustomerDataTests()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: "customer_data_tests")
                .Options;

            _session = new CoreDbContext(options);
        }

        [Fact]
        public void CanGetCustomerById()
        {
            var customer = _session.GetCustomer(1);

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
            var product = _session.GetProduct(1);

            Assert.NotNull(product);
        }
        [Fact]
        public void OrderContainsProduct()
        {
            Assert.True(_session.GetOrder(1).ProductOrders.Any(p => p.Product.Id == 1));
        }
        [Fact]
        public void OrderHasACustomer()
        {
            Assert.NotEmpty(_session.GetOrder(1).Customer.Firstname);
        }



        private static void TestFixtureSetup(DbContextOptions options)
        {
            using (var dbContext=new CoreDbContext(options))
            {
                dbContext.Database.Migrate();
            }
            var doc = XDocument.Load(Path.Combine("TestData", "TestData.xml"));
            var import = new XmlImport(doc, "http://tempuri.org/Database.xsd");
            var customer = new List<Customer>();
            using (var session = new CoreDbContext(options))
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
            using (var session = new CoreDbContext(options))
            {
                import.ParseConnections("OrderProduct", "Product", "Order", (productId, orderId) =>
                {
                    var product = session.Products.Single(p => p.Id == productId);
                    var order = session.Orders.Single(o => o.Id == orderId);
                    session.ProductOrders.Add(new ProductOrder { Order = order, Product = product });
                });

                import.ParseIntProperty("Order", "Customer", (orderId, customerId) =>
                {
                    session.Orders.Single(o => o.Id == orderId).Customer = session.Customers.Single(c => c.Id == customerId);
                });

                session.SaveChanges();
            }
        }

        public void Dispose()
        {
            ((IDisposable)_session).Dispose();
        }
    }
}
