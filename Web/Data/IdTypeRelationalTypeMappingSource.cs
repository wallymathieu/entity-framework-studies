using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace SomeBasicEFApp.Web.Data
{
    public class IdTypeRelationalTypeMappingSource : SqlServerTypeMappingSource 
    {
        private readonly RelationalTypeMapping _customerId=
            new IdTypeRelationalTypeMapping("int", typeof(CustomerId),  DbType.Int32);
        private readonly RelationalTypeMapping _orderId=
            new IdTypeRelationalTypeMapping("int", typeof(OrderId),  DbType.Int32);
        private readonly RelationalTypeMapping _productId=
            new IdTypeRelationalTypeMapping("int", typeof(ProductId),  DbType.Int32);

        private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings;

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
}