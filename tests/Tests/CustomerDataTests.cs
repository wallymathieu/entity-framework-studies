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

namespace SomeBasicEFApp.Tests;

public abstract class CustomerDataTests : IDisposable
{
    private CoreDbContext DbContext;
    public CustomerDataTests() => DbContext = new CoreDbContext(Options);
    public abstract DbContextOptions Options { get; }

    ProductId yoyoId = 1;
    ProductId gumballsId = 2;
    [Fact]
    public void CanGetCustomerById()
    {
        var customer = DbContext.GetCustomer(1);

        Assert.NotNull(customer);
    }

    [Fact]
    public void CanGetCustomerByFirstname()
    {
        var customers = DbContext.Customers
            .Where(c => c.Firstname == "Steve")
            .ToList();
        Assert.Equal(3, customers.Count);
    }

    [Fact]
    public void CanGetProductById()
    {
        var product = DbContext.GetProduct(yoyoId);
        Assert.NotNull(product);
    }
    [Fact]
    public void ProductType()
    {
        Assert.Equal(new ProductType(null), DbContext.GetProduct(yoyoId)?.Type);

        Assert.Equal(new ProductType("Candy"), DbContext.GetProduct(gumballsId)?.Type);
    }
    [Fact]
    public void CanFindProductById()
    {
        var product = DbContext.Find<Product>(yoyoId);
        Assert.NotNull(product);
    }
    [Fact]
    public void OrderContainsProduct()
    {
        OrderId orderId = 1;
        var o = DbContext.Orders
            .Include(order => order.ProductOrders)
            .ThenInclude(po => po.Product)
            .Single(order => order.Id == orderId);
        Assert.NotNull(o.ProductOrders);
        Assert.Contains(o.ProductOrders, p => p.Product?.Id == yoyoId);
    }

    [Fact]
    public void ProductsWithOrders()
    {
        var products = DbContext.Products
                .Include(p => p.ProductOrders)
                .ThenInclude(po => po.Order)
                .WhereThereAreOrders(
                    from: new DateTime(2008, 5, 29),
                    to: new DateTime(2008, 6, 2))
                .ToArray()
            ;
        var orderIds = products
            .SelectMany(p => p.ProductOrders.Select(po => po.Order?.Id))
            .Distinct();
        Assert.Equal(4, Assert.Single(orderIds));
    }

    [Fact]
    public void OrderHasACustomer()
    {
        OrderId orderId = 1;
        var o = DbContext.Orders
            .Include(order => order.Customer)
            .Single(order => order.Id == orderId);
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
                (obj) =>
                {
                    switch (obj)
                    {
                        case Customer c:
                            session.Customers.Add(c);
                            break;
                        case Order o:
                            session.Orders.Add(o);
                            break;
                        case Product p:
                            session.Products.Add(p);
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
                session.ProductOrders.Add(new ProductOrder { Order= order, Product= product });
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

    public void Dispose() => DbContext.Dispose();
}
