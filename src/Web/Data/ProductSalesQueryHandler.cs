using System;
using System.Linq;
using SomeBasicEFApp.Web.Entities;

namespace SomeBasicEFApp.Web.Data
{
    public static class ProductSalesQueryHandler
    {
        /// <summary>
        /// All of the products that has orders associated with them
        /// </summary>
        public static IQueryable<Product> WhereThereAreOrders(
            this IQueryable<Product> self, DateTime @to, DateTime @from) =>
                self.Where(p => p.ProductOrders.Any(po =>
                                                     @from <= po.Order.OrderDate
                                                     && po.Order.OrderDate <= @to));
    }
}
