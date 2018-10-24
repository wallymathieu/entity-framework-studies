module Tests

open System
open Xunit
open FSharp.Data
open Microsoft.EntityFrameworkCore
open WebFs.Domain
open System.IO
open FSharp.Control.Tasks.V2

module TestData=
    open CoreFs

    type TestData = XmlProvider<"../Tests/TestData/TestData.xml", Global=false>

    let fillDb (options:DbContextOptions)=
        use session = new CoreDbContext(options)
        let toCustomer (o : TestData.Customer) =
            Customer(id=o.Id,version=o.Version,firstname=o.Firstname,lastname=o.Lastname)

        let toOrder (o : TestData.Order)=
            Order(id=o.Id,version=o.Version,customer=session.Customers.Find(o.Customer),orderDate=o.OrderDate.DateTime)

        let toProduct (o : TestData.Product)=
            Product(id=o.Id,version=o.Version,name=o.Name,cost=float o.Cost)

        let toOrderProduct(o : TestData.OrderProduct)=
            let order=session.Orders.Find(o.Order)
            let product=session.Products.Find(o.Product)
            (order,product)

        use f = File.Open("TestData/TestData.xml", FileMode.Open, FileAccess.Read, FileShare.Read)
        let db = TestData.Load(f)

        for customer in db.Customers |> Array.map toCustomer do
            session.Add customer |> ignore
        
        for order in db.Orders |> Array.map toOrder do
            session.Add order |> ignore
        for product in db.Products |> Array.map toProduct do
            session.Add product |> ignore
        for (order,product) in db.OrderProducts |> Array.map toOrderProduct do
            order.Products.Add ( ProductOrder (order, product) )
        session.SaveChanges() |> ignore

type CustomerDataTests()=
    let options=
        lazy 
            let db = sprintf "customer_data_tests_%O" <| Guid.NewGuid()
            let ctxB= DbContextOptionsBuilder()
                                .UseInMemoryDatabase(databaseName= db)
                                .Options
                
            TestData.fillDb ctxB
            ctxB
    let session= lazy (new CoreDbContext ( options.Value ))

    [<Fact>]
    member this.CanGetCustomerById()=task{
        let! c = session.Value.Customers.FindAsync 1
        Assert.NotNull c }

    [<Fact>]
    member this.CanGetProductById()= task{
        let! p = session.Value.Products.FindAsync 1
        Assert.NotNull p }

    [<Fact>]
    member this.OrderContainsProduct()=task{
        let! order = session.Value.Orders.IncludeProducts().FirstAsync(fun o->o.OrderId=1)
        Assert.True(order.Products |> Seq.tryFind( fun p -> p.Product.ProductId = 1) |> Option.isSome) }

