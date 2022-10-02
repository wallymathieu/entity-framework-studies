module WebFs.Models

open CoreFs

[<CLIMutable>]
type EditCustomer={Firstname:string; Lastname:string}
[<CLIMutable>]
type EditProduct={Name:string; Cost:float}
[<CLIMutable>]
type AddProductToOrderModel={ProductId:ProductId}

type CustomerModel={Id:CustomerId; Firstname:string; Lastname:string}
and OrderModel={Id:OrderId; Customer:CustomerModel; Products:ProductModel[]}
and ProductModel={Id:ProductId; Name:string; Cost:float}

open Domain
module Mapper=
    open CoreFs

    let mapCustomer (c:Customer) : CustomerModel =
        { Id=c.CustomerId; Firstname=c.Firstname; Lastname=c.Lastname  }
    let mapProduct (p:Product) : ProductModel =
        { Id=p.ProductId; Name=p.ProductName; Cost=p.Cost }
    let mapOrder (o:Order) : OrderModel=
        let customer = if isNull o.Customer then { Id=CustomerId 0; Firstname=""; Lastname="" } else mapCustomer o.Customer
        let products=if isNull o.Products
                     then [||]
                     else o.Products |> Seq.map (fun po->mapProduct po.Product) |> Seq.toArray
        { Id=o.OrderId; Customer=customer; Products= products }
