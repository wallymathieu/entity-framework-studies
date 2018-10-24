using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace SomeBasicEFApp.Tests
{
    public class XmlImport
    {
        XNamespace _ns;
        XDocument xDocument;
        public XmlImport(XDocument xDocument, XNamespace ns)
        {
            _ns = ns;
            this.xDocument = xDocument;
        }
        private object Parse(XElement target, Type type, Action<Type, PropertyInfo> onIgnore)
        {
            var props = type.GetProperties();
            var customerObj = Activator.CreateInstance(type);
            foreach (var propertyInfo in props)
            {
                XElement propElement = target.Element(_ns + propertyInfo.Name);
                if (null != propElement)
                {
                    if (!(propertyInfo.PropertyType.GetTypeInfo().IsValueType || propertyInfo.PropertyType == typeof(string)))
                    {
                        if (onIgnore != null) onIgnore(type, propertyInfo);
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

        public IEnumerable<Tuple<Type, Object>> Parse(IEnumerable<Type> types, Action<Type, Object> onParsedEntity = null, Action<Type, PropertyInfo> onIgnore = null)
        {
            var db = xDocument.Root;
            var list = new List<Tuple<Type, Object>>();

            foreach (var type in types)
            {
                var elements = db.Elements(_ns + type.Name);

                foreach (var element in elements)
                {
                    var obj = Parse(element, type, onIgnore);
                    if (null != onParsedEntity) onParsedEntity(type, obj);
                    list.Add(Tuple.Create(type, obj));
                }
            }
            return list;
        }
        public IEnumerable<Tuple<int, int>> ParseConnections(string name, string first, string second, Action<int, int> onParsedEntity = null)
        {
            var ns = _ns;
            var db = xDocument.Root;
            var elements = db.Elements(ns + name);
            var list = new List<Tuple<int, int>>();
            foreach (var element in elements)
            {
                XElement f = element.Element(ns + first);
                XElement s = element.Element(ns + second);
                var firstValue = int.Parse(f.Value);
                var secondValue = int.Parse(s.Value);
                if (null != onParsedEntity) onParsedEntity(firstValue, secondValue);
                list.Add(Tuple.Create(firstValue, secondValue));
            }
            return list;
        }

        public IEnumerable<Tuple<int, int>> ParseIntProperty(string name, string elementName, Action<int, int> onParsedEntity = null)
        {
            var ns = _ns;
            var db = xDocument.Root;
            var elements = db.Elements(ns + name);
            var list = new List<Tuple<int, int>>();

            foreach (var element in elements)
            {
                XElement f = element.Element(ns + "Id");
                XElement s = element.Element(ns + elementName);
                var id = int.Parse(f.Value);
                var other = int.Parse(s.Value);
                if (null != onParsedEntity) onParsedEntity(id, other);
                list.Add(Tuple.Create(id, other));
            }
            return list;
        }
    }
}
