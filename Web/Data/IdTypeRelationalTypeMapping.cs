using System;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace SomeBasicEFApp.Web.Data
{
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
            return parameter;
        }
    }
}