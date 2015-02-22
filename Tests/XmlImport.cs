using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using SomeBasicEFApp.Core;
using NUnit.Framework;

namespace SomeBasicEFApp.Tests
{
	[TestFixture]
	public class XmlImport
	{

		[Test]
		public void TestParse()
		{
			XNamespace ns = "http://tempuri.org/Database.xsd";
			var file = XDocument.Parse(@"<?xml version=""1.0"" standalone=""yes""?>
<Database xmlns=""http://tempuri.org/Database.xsd"">
  <Customer>
    <Id>1</Id>
    <Firstname>Steve</Firstname>
    <Lastname>Bohlen</Lastname>
    <Version>1</Version>
  </Customer></Database>");
			var cust = (Customer)Parse(file.Root.Elements(ns + typeof(Customer).Name).First(), typeof(Customer), "http://tempuri.org/Database.xsd");
			Assert.That(cust.Id, Is.EqualTo(1));
			Assert.That(cust.Firstname, Is.EqualTo("Steve"));
			Assert.That(cust.Lastname, Is.EqualTo("Bohlen"));
		}

		public static object Parse(XElement target, Type type, XNamespace ns)
		{
			var props = type.GetProperties();
			var customerObj = Activator.CreateInstance(type);
			foreach (var propertyInfo in props)
			{
				XElement propElement = target.Element(ns + propertyInfo.Name);
				if (null != propElement)
				{
					if (!(propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string)))
					{
						Console.WriteLine("ignoring {0} {1}", type.Name, propertyInfo.PropertyType.Name);
					}
					else
					{
						var value = Convert.ChangeType(propElement.Value, propertyInfo.PropertyType, CultureInfo.InvariantCulture.NumberFormat);
						propertyInfo.SetValue(customerObj, value, null);
					}
				}
			}
			return customerObj;
		}

		public static void Parse(XDocument xDocument, IEnumerable<Type> types, Action<Type, Object> onParsedEntity, XNamespace ns)
		{
			var db = xDocument.Root;
			Assert.That(db, Is.Not.Null);

			foreach (var type in types)
			{
				var elements = db.Elements(ns + type.Name);

				foreach (var element in elements)
				{
					var obj = Parse(element, type, ns);
					onParsedEntity(type, obj);
				}
			}
		}
		public static void ParseConnections(XDocument xDocument, string name, string first, string second, Action<int, int> onParsedEntity, XNamespace ns)
		{
			var db = xDocument.Root;
			Assert.That(db, Is.Not.Null);
			var elements = db.Elements(ns + name);

			foreach (var element in elements)
			{
				XElement f = element.Element(ns + first);
				XElement s = element.Element(ns + second);
				onParsedEntity(Int32.Parse(f.Value), Int32.Parse(s.Value));
			}
		}

		public static void ParseIntProperty(XDocument xDocument, string name, string elementName, Action<int, int> onParsedEntity, XNamespace ns)
		{
			var db = xDocument.Root;
			Assert.That(db, Is.Not.Null);
			var elements = db.Elements(ns + name);

			foreach (var element in elements)
			{
				XElement f = element.Element(ns + "Id");
				XElement s = element.Element(ns + elementName);
				onParsedEntity(Int32.Parse(f.Value), Int32.Parse(s.Value));
			}
		}
	}
}
