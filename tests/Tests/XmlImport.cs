using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Tests
{
    public class XmlImport
    {
        readonly XNamespace _ns;
        readonly XDocument xDocument;
        public XmlImport(XDocument xDocument, XNamespace ns)
        {
            if (ns is null) throw new NullReferenceException(nameof(ns));
            if (xDocument is null) throw new NullReferenceException(nameof(xDocument));
            _ns = ns;
            this.xDocument = xDocument;
        }
        /// <summary>pick out constructor with single parateter and construct instance:</summary>
        private static (ConstructorInfo, Type) GetConstructorAndParameterType(Type t)
        {
            var ctor = t.GetConstructors().Single(c => c.GetParameters().Length == 1);
            return (ctor, ctor.GetParameters()[0].ParameterType);
        }
        private object Parse(XElement target, Type type, Action<Type, PropertyInfo>? onIgnore)
        {
            static bool InValueTypesNamespace(Type propertyType) =>
                propertyType.Namespace == typeof(CustomerId).Namespace;

            static bool IsValueTypeOrString(Type propertyType) =>
                propertyType.GetTypeInfo().IsValueType || propertyType == typeof(string)
                                                       || typeof(IValueType).GetTypeInfo().IsAssignableFrom(propertyType);
            var props = type.GetProperties();
            var customerObj = Activator.CreateInstance(type);
            foreach (var propertyInfo in props)
            {
                XElement? propElement = target.Element(_ns + propertyInfo.Name);
                if (null != propElement)
                {
                    if (!IsValueTypeOrString(propertyInfo.PropertyType))
                    {
                        onIgnore?.Invoke(type, propertyInfo);
                    }
                    else if (InValueTypesNamespace(propertyInfo.PropertyType))
                    {
                        var (ctor, parameterType) = GetConstructorAndParameterType(propertyInfo.PropertyType);
                        var value = ctor.Invoke(new[]{Convert.ChangeType(propElement.Value,
                                    parameterType, CultureInfo.InvariantCulture.NumberFormat)});
                        propertyInfo.SetValue(customerObj, value, null);
                    }
                    else
                    {
                        var value = Convert.ChangeType(propElement.Value, propertyInfo.PropertyType, CultureInfo.InvariantCulture.NumberFormat);
                        propertyInfo.SetValue(customerObj, value, null);
                    }
                }
            }
            return customerObj!;
        }

        public IEnumerable<Tuple<Type, object>> Parse(IEnumerable<Type> types, Action<Type, object>? onParsedEntity = null, Action<Type, PropertyInfo>? onIgnore = null)
        {
            var db = xDocument.Root;
            var list = new List<Tuple<Type, object>>();

            foreach (var type in types)
            {
                var elements = db.Elements(_ns + type.Name);

                foreach (var element in elements)
                {
                    var obj = Parse(element, type, onIgnore);
                    onParsedEntity?.Invoke(type, obj);
                    list.Add(Tuple.Create(type, obj));
                }
            }
            return list;
        }
        public IEnumerable<Tuple<int, int>> ParseConnections(string name, string first, string second, Action<int, int>? onParsedEntity = null)
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
                onParsedEntity?.Invoke(firstValue, secondValue);
                list.Add(Tuple.Create(firstValue, secondValue));
            }
            return list;
        }

        public IEnumerable<Tuple<int, int>> ParseIntProperty(string name, string elementName, Action<int, int>? onParsedEntity = null)
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
                onParsedEntity?.Invoke(id, other);
                list.Add(Tuple.Create(id, other));
            }
            return list;
        }
    }
}
