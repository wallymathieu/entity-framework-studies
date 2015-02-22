using System.IO;
using System.Xml.Linq;
using SomeBasicEFApp.Core;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Order = SomeBasicEFApp.Core.Order;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace SomeBasicEFApp.Tests
{
	[TestFixture]
	public class CustomerDataTests
	{

		private CoreDbContext _session;
		private IUnityContainer _unityContainer;


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
			_session = _unityContainer.Resolve<Session>().CreateSession("CustomerDataTests.db");
		}


		[TearDown]
		public void TearDown()
		{
			_session.Dispose();
		}
		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			if (File.Exists("entityframework.mdf"))
			{
				File.Delete("entityframework.mdf");
			}
		}
		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			//CreateDb.CreateSqlDatabase("entityframework.mdf");
			_unityContainer = new UnityContainer().RegisterCore(Runtime.Console);

			new Migrator("entityframework.mdf").Migrate();
			var doc = XDocument.Load(Path.Combine("TestData", "TestData.xml"));
			var customer = new List<Customer>();
			using (var session = _unityContainer.Resolve<Session>().CreateSession("entityframework.mdf"))
			{
				XmlImport.Parse(doc, new[] { typeof(Customer), typeof(Order), typeof(Product) },
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

								}, "http://tempuri.org/Database.xsd");
			}
			using (var session = _unityContainer.Resolve<Session>().CreateSession("entityframework.mdf"))
			{

				XmlImport.ParseConnections(doc, "OrderProduct", "Product", "Order", (productId, orderId) =>
				{
					var product = session.Products.Single(p => p.Id == productId);
					var order = session.Orders.Single(o => o.Id == orderId);
					order.Products.Add(product);
				}, "http://tempuri.org/Database.xsd");

				XmlImport.ParseIntProperty(doc, "Order", "Customer",
				(orderId, customerId) =>
				{
					session.Orders.Single(o => o.Id == orderId).Customer = session.Customers.Single(c => c.Id == customerId);
				}, "http://tempuri.org/Database.xsd");

				session.SaveChanges();
			}
		}

	}
}
