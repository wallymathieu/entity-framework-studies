module Tests

open System
open Xunit
open FSharp.Data
open Microsoft.EntityFrameworkCore
open EntityFrameworkCore.FSharp.DbContextHelpers
open WebFs.Domain
open System.IO
open System.Linq
open CoreFs

module TestData=
    open CoreFs

    type TestData = XmlProvider<"../Tests/TestData/TestData.xml", Global=false>

    let fillDb (options:DbContextOptions)=
        use session = new CoreDbContext(options)
        let toCustomer (o : TestData.Customer) = { Customer.Default with
                                                    CustomerId = CustomerId o.Id
                                                    Version = o.Version
                                                    Firstname = o.Firstname
                                                    Lastname = o.Lastname }
        let toOrder (o : TestData.Order) = { Order.Create o.OrderDate.DateTime (findEntity session (CustomerId o.Customer)) with
                                              OrderId = OrderId o.Id
                                              Version = o.Version }

        let toProduct (o : TestData.Product) = { Product.Default with
                                                  ProductId = ProductId o.Id
                                                  Version = o.Version
                                                  ProductName = o.Name
                                                  Cost = float o.Cost }

        use f = File.Open("TestData/TestData.xml", FileMode.Open, FileAccess.Read, FileShare.Read)
        let db = TestData.Load(f)

        for customer in db.Customers |> Array.map toCustomer do
            session.Add customer |> ignore

        for order in db.Orders |> Array.map toOrder do
            session.Add order |> ignore
        for product in db.Products |> Array.map toProduct do
            session.Add product |> ignore
        session.SaveChanges() |> ignore
        use session = new CoreDbContext(options)
        let toOrderProduct(o : TestData.OrderProduct)=
            let orderId =OrderId o.Order
            let order=session.Orders.IncludeProducts().Single(fun o1->o1.OrderId= orderId)
            let product=session.Products.Find(ProductId o.Product)
            (order,product)
        for (order,product) in db.OrderProducts |> Array.map toOrderProduct do
            let orderProduct = ProductOrder.Create (order, product)
            order.Products.Add orderProduct
        session.SaveChanges() |> ignore
let yoyoId= ProductId 1
[<AbstractClass>]
type CustomerDataTests()=
    abstract member Options : DbContextOptions
    member this.Session = new CoreDbContext (this.Options)

    [<Fact>]
    member this.Can_get_customer_by_id()=task{
        let! cs = this.Session.Customers.ToArrayAsync()
        let! c = this.Session.Customers.FindAsync (CustomerId 1)
        Assert.NotNull c }

    [<Fact>]
    member this.Can_get_product_by_id()= task{
        let! p = this.Session.Products.FindAsync yoyoId
        Assert.NotNull p }

    [<Fact>]
    member this.Order_contains_product()=task{
        let orderId= OrderId 1
        let! order = this.Session.Orders.IncludeProducts().FirstAsync(fun o->o.OrderId=orderId)
        Assert.Contains(order.Products, fun p -> p.Product.ProductId = yoyoId) }

module InMemory=
    let fixtureOptions=lazy(
        let db = sprintf "customer_data_tests_%O" <| Guid.NewGuid()
        let options= DbContextOptionsBuilder()
                            .UseInMemoryDatabase(databaseName= db)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors()
                            .Options
        TestData.fillDb options
        options)

(* Somehow this doesn't work with typed ids
type InMemoryCustomerDataTests()=
    inherit CustomerDataTests()
    default this.Options = InMemory.fixtureOptions.Value *)
module SqlLite=
    open FsMigrations
    let private createOptions ()=
        if File.Exists "CoreFsTests.db" then
            File.Delete "CoreFsTests.db"

        let options= DbContextOptionsBuilder()
                            .UseSqlite("Data Source=CoreFsTests.db")
                            .Options
        let runner = MigrationRunner.create "Data Source=CoreFsTests.db" "SQLite"
        runner.MigrateUp()
        TestData.fillDb options
        options
    let fixtureOptions=lazy createOptions ()

type SqlLiteCustomerDataTests()=
    inherit CustomerDataTests()
    default this.Options = SqlLite.fixtureOptions.Value
