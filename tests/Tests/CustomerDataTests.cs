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
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Tests
{
    public abstract class CustomerDataTests:IDisposable
    {
        private CoreDbContext Session;
        public CustomerDataTests()
        {
            Session = new CoreDbContext(Options);
        }
        public abstract DbContextOptions Options { get; }

        [Fact]
        public void CanGetCustomerById()
        {
            var customer = Session.GetCustomer(1);

            Assert.NotNull(customer);
        }

        [Fact]
        public void CanGetCustomerByFirstname()
        {
            var customers = Session.Customers
                .Where(c => c.Firstname == "Steve")
                .ToList();
            Assert.Equal(3, customers.Count);
        }

        [Fact]
        public void CanGetProductById()
        {
            var product = Session.GetProduct(1);
            Assert.Equal(new ProductType("Toy"), product.Type);

            Assert.NotNull(product);
            product.Type=new ProductType("Other");
            Session.SaveChanges();
            Assert.Equal(new ProductType("Other"), Session.GetProduct(1).Type);
        }
        
        [Fact]
        public void OrderContainsProduct()
        {
            var o = Session.Orders
                    .Include(order=>order.ProductOrders)
                        .ThenInclude(po=>po.Product)
                    .Single(order=>order.Id==1);
            Assert.NotNull(o.ProductOrders);
            Assert.True(o.ProductOrders.Any(p => p.Product.Id == 1));
        }

        [Fact]
        public void ProductsWithOrders()
        {
            var products = Session.Products
                                 .Include(p=>p.ProductOrders)
                                 .ThenInclude(po => po.Order)
                                 .WhereThereAreOrders(
                                    from:new DateTime(2008, 5, 29), 
                                    to:new DateTime(2008, 6, 2))
                                 .ToArray()
                                 ;
            var orderIds = products
                            .SelectMany(p => p.ProductOrders.Select(po => po.Order.Id))
                            .Distinct();
            Assert.Equal(4, Assert.Single(orderIds));
        }

        [Fact]
        public void OrderHasACustomer()
        {
            var o = Session.Orders
                    .Include(order=>order.Customer)
                    .Single(order=>order.Id==1);
            Assert.NotNull(o.Customer);
            Assert.NotEmpty(o.Customer.Firstname);
        }

        

        public static DbContextOptions Setup(DbContextOptions options)
        {
            var doc = XDocument.Load(Path.Combine(
                Path.GetDirectoryName(typeof(CustomerDataTests).GetTypeInfo().Assembly.Location),
                "TestData", "TestData.xml"));
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
                    var product = session.GetProduct(productId);
                    var order = session.GetOrder(orderId);
                    session.ProductOrders.Add(new ProductOrder { Order = order, Product = product });
                });

                import.ParseIntProperty("Order", "Customer", (orderId, customerId) =>
                {
                    var order = session.GetOrder(orderId);
                    order.Customer = session.GetCustomer(customerId);
                });

                session.SaveChanges();
            }
            return options;
        }

        public void Dispose()
        {
            ((IDisposable)Session).Dispose();
        }
    }
}
