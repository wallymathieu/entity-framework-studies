using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using SomeBasicEFApp.Web.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace SomeBasicEFApp.Web.Data
{
    public class CoreDbCoreConventionSetBuilder : CoreConventionSetBuilder
    {
        public CoreDbCoreConventionSetBuilder(CoreConventionSetBuilderDependencies dependencies) : base(dependencies)
        {
        }
        public override Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet CreateConventionSet()
        {
            var @set = base.CreateConventionSet();
            return @set;
        }
    }
    public class IdTypeRelationalTypeMappingSource : SqlServerTypeMappingSource 
    {
        private RelationalTypeMapping _customerId=
            new IdTypeRelationalTypeMapping("int", typeof(CustomerId),  DbType.Int32);
        private RelationalTypeMapping _orderId=
            new IdTypeRelationalTypeMapping("int", typeof(OrderId),  DbType.Int32);
        private RelationalTypeMapping _productId=
            new IdTypeRelationalTypeMapping("int", typeof(ProductId),  DbType.Int32);

        private Dictionary<Type, RelationalTypeMapping> _clrTypeMappings;

        public IdTypeRelationalTypeMappingSource(TypeMappingSourceDependencies dependencies, RelationalTypeMappingSourceDependencies relationalDependencies) : base(dependencies, relationalDependencies)
        {
            _clrTypeMappings
                = new Dictionary<Type, RelationalTypeMapping>
                {
                    { typeof(CustomerId), _customerId },
                    { typeof(OrderId), _orderId},
                    { typeof(ProductId), _productId},
                };
        }

        protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo)
            => FindRawMapping(mappingInfo)?.Clone(mappingInfo)
               ?? base.FindMapping(mappingInfo);
        

        private RelationalTypeMapping FindRawMapping(RelationalTypeMappingInfo mappingInfo)
        {
            var clrType = mappingInfo.ClrType;
            if (clrType != null)
            {
                if (_clrTypeMappings.TryGetValue(clrType, out var mapping))
                {
                    return mapping;
                }
            }

            return null;
        }
    }
    public class IdTypeRelationalTypeMapping : RelationalTypeMapping
    {
        public IdTypeRelationalTypeMapping(string storeType, Type clrType, DbType? dbType = null, bool unicode = false, int? size = null) : base(storeType, clrType, dbType, unicode, size)
        {
        }

        public override RelationalTypeMapping Clone(string storeType, int? size)
        {
            return new IdTypeRelationalTypeMapping(storeType,this.ClrType,this.DbType,this.IsUnicode, size);
        }
        protected override string GenerateNonNullSqlLiteral(object value)
        {
            return ((IId)value).Value.ToString();
        }
        public override DbParameter CreateParameter(DbCommand command, string name, object value, bool? nullable = null)
        {
            var parameter = command.CreateParameter();
            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = name;
            switch (value)
            {
                case null:
                    parameter.Value = DBNull.Value;
                    break;
                case IId id:
                    parameter.Value = id.Value;
                    break;
            }
            if (nullable.HasValue)
            {
                parameter.IsNullable = nullable.Value;
            }

            if (DbType.HasValue)
            {
                parameter.DbType = DbType.Value;
            }
            return base.CreateParameter(command, name, value, nullable);
        }
    }

    public partial class CoreDbContext : IdentityDbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options) {}
        public CoreDbContext() {}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductOrder>().HasKey(b => new { b.OrderId, b.ProductId });
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public Customer GetCustomer(CustomerId v)
        {
            return Customers.SingleOrDefault(c => c.Id == v);
        }
        public Product GetProduct(ProductId v)
        {
            return Products.SingleOrDefault(p => p.Id == v);
        }
        public Order GetOrder(OrderId v)
        {
            return Orders.SingleOrDefault(o => o.Id == v);
        }
    }
}
