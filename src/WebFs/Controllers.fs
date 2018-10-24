namespace WebFs.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open FSharp.Control.Tasks.V2

open CoreFs

open WebFs.Domain
open WebFs.Models
type AR = IActionResult
type HomeController () =
    inherit Controller()
    [<HttpGet("")>]
    member this.Index() = this.Redirect "/swagger"

[<Route("/api/v1/customers")>]
[<ApiController>]
[<ApiExplorerSettings(GroupName = "v1")>]
type CustomersController (context:CoreDbContext) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() = task{ // here you normally want filtering based on query parameters (in order to get better perf)
        let! result= context.Customers.Select(Mapper.mapCustomer).ToListAsync()
        return ActionResult<_>(result)
    }

    [<HttpGet("{id}")>]
    member this.Get(id:int) =task{
        let! customer = context.Customers.FindAsync(id)
        return 
            if isNull customer then this.NotFound() :> AR
            else this.Ok(Mapper.mapCustomer customer) :> AR
    }

    [<HttpPost>]
    member this.Post([<FromBody>] value:EditCustomer) =task{
         let customer = Customer()
         customer.Lastname <- value.Lastname
         customer.Firstname <- value.Firstname
         let! _ = context.AddAsync customer
         let! _ = context.SaveChangesAsync()
         return this.Ok(Mapper.mapCustomer customer) :> AR
    }

    [<HttpPut("{id}")>]
    member this.Put(id:int, [<FromBody>] value:EditCustomer ) =task{
        let! customer = context.Customers.FindAsync(id)
        if isNull customer then return this.NotFound() :> AR
        else
            customer.Lastname <- value.Lastname
            customer.Firstname <- value.Firstname
            let! _ = context.SaveChangesAsync()
            return this.Ok(Mapper.mapCustomer customer) :> AR
    }

    [<HttpDelete("{id}")>]
    member this.Delete(id:int) =task{
        let! customer = context.Customers.FindAsync(id)
        if isNull customer then return this.NotFound() :> AR
        else
            context.Customers.Remove customer |> ignore
            let! _ = context.SaveChangesAsync()
            return this.Ok(Mapper.mapCustomer customer) :> AR
    }

[<Route("/api/v1/orders")>]
[<ApiController>]
[<ApiExplorerSettings(GroupName = "v1")>]
type OrdersController (context:CoreDbContext) =
    inherit ControllerBase()

    [<HttpGet("")>]
    member this.Index() = task { // here you normally want filtering based on query parameters (in order to get better perf)
        let! orders=context.Orders
                        .Include(fun o->o.Customer)
                        .IncludeProducts()
                        .ToListAsync()
                        

        return this.Ok (orders |> Seq.map Mapper.mapOrder)
    }

    [<HttpPost("")>]
    member this.Post() = task {
        let order = Order()
        order.OrderDate <- DateTime.UtcNow
        let! _ = context.AddAsync order
        let! _ = context.SaveChangesAsync()
        return this.Ok (Mapper.mapOrder order)
    }

    [<HttpPost("{id}/products")>]
    member this.PostProduct(id:int, [<FromBody>] body:AddProductToOrderModel) = task {
        let! order=context.Orders
                        .Include(fun o->o.Customer)
                        .IncludeProducts()
                        .FirstOrDefaultAsync(fun o->o.OrderId=id)
        let! product=context.Products.FindAsync body.ProductId
        if (isNull order) then return this.NotFound() :> AR
        else
            order.Products.Add (ProductOrder (order, product))
            let! _ = context.SaveChangesAsync()
            return this.Ok (Mapper.mapOrder order) :> AR
    }

[<Route("/api/v1/products")>]
[<ApiController>]
[<ApiExplorerSettings(GroupName = "v1")>]
type ProductsController (context:CoreDbContext) =
    inherit ControllerBase()

    [<HttpGet("")>]
    member this.Index() = task{ // here you normally want filtering based on query parameters (in order to get better perf)
        let! products=context.Products
                        .Select(Mapper.mapProduct)
                        .ToListAsync();
        return this.Ok products
    }
    [<HttpPost>]
    member this.Post([<FromBody>] value:EditProduct) =task{
         let product = Product()
         product.ProductName <- value.Name
         product.Cost <- value.Cost
         let! _ = context.AddAsync product
         let! _ = context.SaveChangesAsync()
         return this.Ok(Mapper.mapProduct product) :> AR
    }
    [<HttpPut("{id}")>]
    member this.Put(id:int,[<FromBody>] value:EditProduct) =task{
         let! product = context.Products.FindAsync(id)
         product.ProductName <- value.Name
         product.Cost <- value.Cost
         let! _ = context.SaveChangesAsync()
         return this.Ok(Mapper.mapProduct product) :> AR
    }
