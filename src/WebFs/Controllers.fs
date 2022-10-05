namespace WebFs.Controllers

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open EntityFrameworkCore.FSharp.DbContextHelpers
open CoreFs
open WebFs.Domain
open WebFs.Models
type AR = IActionResult
[<ApiExplorerSettings(GroupName = "none")>]
type HomeController () =
    inherit Controller()
    [<HttpGet("")>]
    member this.Index() = this.Redirect "/swagger"

[<Route("/api/v1/customers");
  ApiController;
  ApiExplorerSettings(GroupName = "v1")>]
type CustomersController (context: CoreDbContext) =
    inherit ControllerBase()

    [<HttpGet;
      Produces(typeof<Customer[]>)>]
    member this.Get() = task { // here you normally want filtering based on query parameters (in order to get better perf)
        let! result= context.Customers.ToListAsync()
        return ActionResult<_>(result) }

    [<HttpGet("{id}");
      Produces(typeof<Customer>)>]
    member this.Get([<FromRoute>] id: CustomerId) = async {
        match! tryFindEntityAsync context id with
        | Some (customer:Customer) -> return this.Ok(customer) :> AR
        | None -> return this.NotFound() :> AR }

    [<HttpPost;
      Produces(typeof<Customer>)>]
    member this.Post([<FromBody>] value: EditCustomer) = async {
         let customer = { Customer.Default with Lastname =value.Lastname; Firstname=value.Firstname }
         do! addEntityAsync context customer
         do! saveChangesAsync context
         return this.Ok(customer) :> AR }

    [<HttpPut("{id}");
      Produces(typeof<Customer>)>]
    member this.Put([<FromRoute>] id: CustomerId, [<FromBody>] value: EditCustomer ) = async {
        match! tryFindEntityAsync context id with
        | Some (customer:Customer) ->
            customer.Lastname <- value.Lastname
            customer.Firstname <- value.Firstname
            do! saveChangesAsync context
            return this.Ok(customer) :> AR
        | None -> return this.NotFound() :> AR }

    [<HttpDelete("{id}");
      Produces(typeof<Customer>)>]
    member this.Delete([<FromRoute>] id: CustomerId) = async {
        match! tryFindEntityAsync context id with
        | Some (customer:Customer) ->
            removeEntity context customer
            do! saveChangesAsync context
            return this.Ok(customer) :> AR
        | None -> return this.NotFound() :> AR }

[<Route("/api/v1/orders");
  ApiController;
  ApiExplorerSettings(GroupName = "v1")>]
type OrdersController (context: CoreDbContext) =
    inherit ControllerBase()

    [<HttpGet("");
      Produces(typeof<Order[]>)>]
    member this.Index() = task { // here you normally want filtering based on query parameters (in order to get better perf)
        let! orders=context.Orders
                        .Include(fun o->o.Customer)
                        .IncludeProducts()
                        .ToListAsync()
        return this.Ok orders }

    [<HttpPost("");
      Produces(typeof<Order>)>]
    member this.Post() = async {
        let order = Order.Create DateTime.UtcNow Unchecked.defaultof<_>
        do! addEntityAsync context order
        do! saveChangesAsync context
        return this.Ok order }

    [<HttpPost("{id}/products");
      Produces(typeof<Order>)>]
    member this.PostProduct([<FromRoute>] id:OrderId, [<FromBody>] body:AddProductToOrderModel) = async {
        let! maybeOrder =
                    context.Orders
                        .Include(fun o->o.Customer)
                        .IncludeProducts()
                        .TryFirstAsync(fun o->o.OrderId=id)
        let! maybeProduct = tryFindEntityAsync context body.ProductId
        match (maybeOrder,maybeProduct) with
        | None,_
        | _,None
             -> return this.NotFound() :> AR
        | Some (order:Order), Some (product:Product) ->
            do! addEntityAsync context (ProductOrder.Create (order, product))
            do! saveChangesAsync context
            return this.Ok order :> AR }

[<Route("/api/v1/products");
  ApiController;
  ApiExplorerSettings(GroupName = "v1")>]
type ProductsController (context: CoreDbContext) =
    inherit ControllerBase()

    [<HttpGet("");
      Produces(typeof<Product[]>)>]
    member this.Index() = task{ // here you normally want filtering based on query parameters (in order to get better perf)
        let! products=context.Products.ToListAsync()
        return this.Ok products }
    [<HttpGet("{id}");
      Produces(typeof<Product>)>]
    member this.Get([<FromRoute>] id:ProductId) = async {
        match! tryFindEntityAsync context id with
        | Some (product:Product) -> return this.Ok(product) :> AR
        | None -> return this.NotFound() :> AR }
    [<HttpPost;
      Produces(typeof<Product>)>]
    member this.Post([<FromBody>] value:EditProduct) = async {
         let product = { Product.Default with ProductName=value.Name; Cost=value.Cost }
         do! addEntityAsync context product
         do! saveChangesAsync context
         return this.Ok(product) :> AR }
    [<HttpPut("{id}");
      Produces(typeof<Product>)>]
    member this.Put([<FromRoute>] id:ProductId, [<FromBody>] value:EditProduct) = async {
        match! tryFindEntityAsync context id with
        | Some (product:Product) ->
            product.ProductName <- value.Name
            product.Cost <- value.Cost
            do! saveChangesAsync context
            return this.Ok(product) :> AR
        | None -> return this.NotFound() :> AR }
