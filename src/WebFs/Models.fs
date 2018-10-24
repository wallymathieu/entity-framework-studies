module WebFs.Models
type EditCustomer={Firstname:string; Lastname:string}

type CustomerModel={Id:string;Firstname:string; Lastname:string}
and OrderModel={Id:string;Customer:CustomerModel;Products:ProductModel[]}
and ProductModel={Id:string; Name:string; Cost:float}

open Domain
module Mapper=
    let mapCustomer (c:Customer) : CustomerModel = 
        { Id=string c.CustomerId; Firstname=c.Firstname; Lastname=c.Lastname  }
    let mapProduct (p:Product) : ProductModel = 
        { Id=string p.ProductId; Name=p.ProductName; Cost=p.Cost }
    let mapOrder (o:Order) : OrderModel=
        let customer = mapCustomer o.Customer
        let products=if isNull o.Products 
                     then [||] 
                     else o.Products |> Seq.map (fun po->mapProduct po.Product) |> Seq.toArray
        { Id=string o.OrderId; Customer=customer; Products= products }
