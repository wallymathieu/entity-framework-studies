using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace SomeBasicEFApp.Web.Data
{
    public class IdTypeServerValueGeneratorConvention : SqlServerValueGeneratorConvention
    {
        
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override Annotation Apply(InternalPropertyBuilder propertyBuilder, string name, Annotation annotation, Annotation oldAnnotation)
        {
            if (name == SqlServerAnnotationNames.ValueGenerationStrategy)
            {
                propertyBuilder.ValueGenerated(GetValueGenerated(propertyBuilder.Metadata), ConfigurationSource.Convention);
                return annotation;
            }

            return base.Apply(propertyBuilder, name, annotation, oldAnnotation);
        }

        
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override ValueGenerated? GetValueGenerated(Property property)
        {
            var valueGenerated = base.GetValueGenerated(property);
            return valueGenerated != null
                ? valueGenerated
                : property.SqlServer().GetSqlServerValueGenerationStrategy(fallbackToModel: false) != null
                    ? ValueGenerated.OnAdd
                    : (ValueGenerated?)null;
        }
    }
    
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
