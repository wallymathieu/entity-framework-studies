namespace CoreFs

open System
open System.Collections.Generic

[<AllowNullLiteral>]
type ProductOrder(order,product)=
    new()=ProductOrder(null, null)
    member val Order : Order=order with get, set
    member val Product : Product=product with get, set
and [<AllowNullLiteral>] Order(id,orderDate,customer,version) =
    new()=Order(0,DateTime.MinValue,null,0)
    member val OrderId =id with get, set
    member val OrderDate =orderDate with get, set
    member val Customer : Customer=customer with get, set
    member val Products =List<ProductOrder>() with get, set
    member val Version = version with get, set
    
and [<AllowNullLiteral>] Customer(id,firstname,lastname,version)=
    new()=Customer(0,"","",0)
    member val CustomerId  =id with get, set
    member val Firstname =firstname with get, set
    member val Lastname =lastname with get, set
    member val Orders = List<Order>() with get, set
    member val Version = version with get, set
and [<AllowNullLiteral>] Product(id,cost,name,version)=
    new()=Product(0,0.0f,"",0)
    member val ProductId =id with get, set
    member val Cost =cost with get, set
    member val ProductName = name with get, set
    member val Orders = List<ProductOrder>() with get, set
    member val Version = version with get, set
