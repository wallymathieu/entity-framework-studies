﻿using System.IO;
using System.Xml.Linq;
using SomeBasicEFApp.Core;
using NUnit.Framework;
using Order = SomeBasicEFApp.Core.Order;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomeBasicEFApp.Tests
{
	[TestFixture]
	public class CustomerDataTests
	{

		private CoreDbContext _session;


		[Test]
		public void CanGetCustomerById()
		{
			var customer = _session.Customers.SingleOrDefault(c => c.Id == 1);

			Assert.IsNotNull(customer);
		}

		[Test]
		public void CanGetCustomerByFirstname()
		{
			var customers = _session.Customers
				.Where(c => c.Firstname == "Steve")
				.ToList();
			Assert.AreEqual(3, customers.Count);
		}

		[Test]
		public void CanGetProductById()
		{
			var product = _session.Products.SingleOrDefault(c => c.Id == 1);

			Assert.IsNotNull(product);
		}

		[SetUp]
		public void Setup()
		{
			_session = new Session(new ConsoleMapPath()).CreateSession("CustomerDataTests.db");
		}


		[TearDown]
		public void TearDown()
		{
			if (_session != null)
			{
				_session.Dispose();
			}
		}

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			if (File.Exists("CustomerDataTests.db")) { File.Delete("CustomerDataTests.db"); }

			new Migrator("CustomerDataTests.db").Migrate();
			var sessions = new Session(new ConsoleMapPath());
			var doc = XDocument.Load(Path.Combine("TestData", "TestData.xml"));
			var import = new XmlImport(doc, "http://tempuri.org/Database.xsd");
			var customer = new List<Customer>();
			using (var session = sessions.CreateSession("CustomerDataTests.db"))
			{
				import.Parse(new[] { typeof(Customer), typeof(Order), typeof(Product) },
								(type, obj) =>
								{
									switch (type.Name)
									{
										case "Customer":
											session.Customers.Add((Customer)obj);
											break;
										case "Order":
											session.Orders.Add((Order)obj);
											break;
										case "Product":
											session.Products.Add((Product)obj);
											break;
										default:
											break;
									}

								});
				session.SaveChanges();
			}
			using (var session = sessions.CreateSession("CustomerDataTests.db"))
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

	}
}
