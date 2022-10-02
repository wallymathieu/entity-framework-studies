namespace WebFs.Controllers

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open CoreFs

open WebFs.Domain
open WebFs.Models
type AR = IActionResult
[<ApiExplorerSettings(GroupName = "none")>]
type HomeController () =
    inherit Controller()
    [<HttpGet("")>]
    member this.Index() = this.Redirect "/swagger"

[<Route("/api/v1/customers")>]
[<ApiController>]
[<ApiExplorerSettings(GroupName = "v1")>]
type CustomersController (context:ICoreDbContext) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() = task{ // here you normally want filtering based on query parameters (in order to get better perf)
        let! result= context.Customers.ToListAsync()
        return ActionResult<_>(result) }

    [<HttpGet("{id}")>]
    member this.Get([<FromRoute>] id:CustomerId) =task{
        let! customer = context.Customers.FindAsync id
        return
            if isNull customer then this.NotFound() :> AR
            else this.Ok(customer) :> AR }

    [<HttpPost>]
    member this.Post([<FromBody>] value:EditCustomer) =task{
         let customer = Customer(customerId= CustomerId 0,lastname =value.Lastname, firstname=value.Firstname, version=0)
         //customer.Lastname <- value.Lastname
         //customer.Firstname <- value.Firstname
         let! _ = context.AddAsync customer
         do! context.SaveChangesAsync()
         return this.Ok(customer) :> AR }

    [<HttpPut("{id}")>]
    member this.Put([<FromRoute>] id:CustomerId, [<FromBody>] value:EditCustomer ) =task{
        let! customer = context.Customers.FindAsync id
        if isNull customer then return this.NotFound() :> AR
        else
            customer.Lastname <- value.Lastname
            customer.Firstname <- value.Firstname
            do! context.SaveChangesAsync()
            return this.Ok(customer) :> AR }

    [<HttpDelete("{id}")>]
    member this.Delete([<FromRoute>] id:CustomerId) =task{
        let! customer = context.Customers.FindAsync id
        if isNull customer then return this.NotFound() :> AR
        else
            context.Customers.Remove customer |> ignore
            do! context.SaveChangesAsync()
            return this.Ok(customer) :> AR }

[<Route("/api/v1/orders")>]
[<ApiController>]
[<ApiExplorerSettings(GroupName = "v1")>]
type OrdersController (context:ICoreDbContext) =
    inherit ControllerBase()

    [<HttpGet("")>]
    member this.Index() = task { // here you normally want filtering based on query parameters (in order to get better perf)
        let! orders=context.Orders
                        .Include(fun o->o.Customer)
                        .IncludeProducts()
                        .ToListAsync()
        return this.Ok orders }

    [<HttpPost("")>]
    member this.Post() = task {
        let order = Order(orderId=OrderId 0, orderDate=DateTime.UtcNow, customer=null, version=0)
        do! context.AddAsync order
        do! context.SaveChangesAsync()
        return this.Ok order }

    [<HttpPost("{id}/products")>]
    member this.PostProduct([<FromRoute>] id:OrderId, [<FromBody>] body:AddProductToOrderModel) = task {
        let! order=context.Orders
                        .Include(fun o->o.Customer)
                        .IncludeProducts()
                        .FirstOrDefaultAsync(fun o->o.OrderId=id)
        let! product=context.Products.FindAsync body.ProductId
        if (isNull order) then return this.NotFound() :> AR
        else
            order.Products.Add (ProductOrder (order, product))
            do! context.SaveChangesAsync()
            return this.Ok order :> AR }

[<Route("/api/v1/products")>]
[<ApiController>]
[<ApiExplorerSettings(GroupName = "v1")>]
type ProductsController (context:ICoreDbContext) =
    inherit ControllerBase()

    [<HttpGet("")>]
    member this.Index() = task{ // here you normally want filtering based on query parameters (in order to get better perf)
        let! products=context.Products.ToListAsync()
        return this.Ok products }
    [<HttpGet("{id}")>]
    member this.Get([<FromRoute>] id:ProductId) =task{
        let! product = context.Products.FindAsync id
        return
            if isNull product then this.NotFound() :> AR
            else this.Ok(product) :> AR }
    [<HttpPost>]
    member this.Post([<FromBody>] value:EditProduct) =task{
         let product = Product(productId=ProductId 0,productName=value.Name, cost=value.Cost, version=0)
         do! context.AddAsync product
         do! context.SaveChangesAsync()
         return this.Ok(product) :> AR }
    [<HttpPut("{id}")>]
    member this.Put([<FromRoute>] id:ProductId, [<FromBody>] value:EditProduct) =task{
         let! product = context.Products.FindAsync id
         product.ProductName <- value.Name
         product.Cost <- value.Cost
         do! context.SaveChangesAsync()
         return this.Ok(product) :> AR }
