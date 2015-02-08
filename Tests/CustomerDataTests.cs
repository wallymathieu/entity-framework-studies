using System.IO;
using System.Xml.Linq;
using SomeBasicEFApp.Core;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Order = SomeBasicEFApp.Core.Order;
using System.Data.Entity;
using System.Linq;

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
            _session = _unityContainer.Resolve<Session>().CreateTestSessionFactory("CustomerDataTests.db");
        }


        [TearDown]
        public void TearDown()
        {
            _session.Dispose();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            _unityContainer = new UnityContainer().RegisterCore(Runtime.Console);
            //if (File.Exists("CustomerDataTests.db")) { File.Delete("CustomerDataTests.db"); }

            using (var session = _unityContainer.Resolve<Session>().CreateTestSessionFactory("CustomerDataTests.db"))
            {
                XmlImport.Parse(XDocument.Load(Path.Combine("TestData", "TestData.xml")), new[] { typeof(Customer), typeof(Order), typeof(Product) },
                                (type, obj) => {
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
                session.SaveChanges();
            }
        }

    }
}
