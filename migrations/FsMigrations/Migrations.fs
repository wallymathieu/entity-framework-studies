namespace FsMigrations.Migrations
open FluentMigrator
open FsMigrations.Extensions
[<Migration(201810241337L)>]
type AddTables()=
    inherit AutoReversingMigration()
    
    default self.Up()=
        self.Create.Table("Customers")
            .WithIdColumn()
            .WithVersionColumn()
            .WithColumn("Firstname").AsString()
            .WithColumn("Lastname").AsString() |> ignore
                          
        self.Create.Table("Orders")
            .WithIdColumn()
            .WithVersionColumn()
            .WithColumn("OrderDate").AsDateTime()
            .WithColumn("CustomerId").AsInt32()
                .Nullable() |> ignore

        self.Create.Table("OrdersToProducts")
            .WithColumn("OrderId").AsInt32().NotNullable()
            .WithColumn("ProductId").AsInt32().NotNullable() |> ignore

        self.Create.Table("Products")
            .WithIdColumn()
            .WithVersionColumn()
            .WithColumn("Cost").AsDouble()
            .WithColumn("Name").AsString() |> ignore
