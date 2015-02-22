#region License
// 
// Copyright (c) 2007-2009, Sean Chambers <schambers80@gmail.com>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using FluentMigrator;

namespace SomeBasicEFApp.DbMigrations
{
    [Migration(20110404074742)]
    public class AddTables : Migration
    {
        public override void Up()
        {
            Create.Table("Customers")
                .WithIdColumn()
                .WithVersionColumn()
                .WithColumn("FirstName").AsString()
                .WithColumn("LastName").AsString();

            Create.Table("Orders")
                .WithIdColumn()
                .WithVersionColumn()
                .WithColumn("OrderDate").AsDateTime()
                .WithColumn("Customer_id").AsInt32().Nullable();

            Create.Table("ProductOrders")
				.WithIdColumn()
				.WithColumn("Order_id").AsInt32().NotNullable()
                .WithColumn("Product_id").AsInt32().NotNullable()
                ;

            Create.Table("Products")
                .WithIdColumn()
                .WithVersionColumn()
                .WithColumn("Cost").AsFloat()
                .WithColumn("Name").AsString();
        }

        public override void Down()
        {
            Delete.Table("Customers");
            Delete.Table("Orders");
            Delete.Table("ProductOrders");
            Delete.Table("Products");
        }
    }
}