namespace CoreFs

open System
open System.Collections.Generic
open System.ComponentModel
open Saithe

[<TypeConverter(typeof<ParseTypeConverter<OrderId>>)>]
type OrderId = OrderId of int
with override this.ToString()= match this with OrderId id->id.ToString()
     
type ProductId = ProductId of int
with override this.ToString()= match this with ProductId id->id.ToString()
type CustomerId = CustomerId of int
with override this.ToString()= match this with CustomerId id->id.ToString()
module OrderId =begin let unwrap (OrderId id)=id end
module ProductId =begin let unwrap (ProductId id)=id end
module CustomerId =begin let unwrap (CustomerId id)=id end

[<AllowNullLiteral>]
type ProductOrder(order:Order,product:Product)=
    new()=ProductOrder(null, null)
    member val Order : Order=order with get, set
    //member val OrderId = if isNull order then OrderId 0 else order.OrderId with get, set
    member val Product : Product=product with get, set
    //member val ProductId = if isNull product then ProductId 0 else product.ProductId with get, set
and [<AllowNullLiteral>] Order(orderId:OrderId,orderDate:DateTime,customer:Customer,version:int) =
    new()=Order(OrderId 0,DateTime.MinValue,null,0)
    member val OrderId =orderId with get, set
    member val OrderDate =orderDate with get, set
    member val Customer : Customer=customer with get, set
    member val Products =List<ProductOrder>() with get, set
    member val Version = version with get, set

and [<AllowNullLiteral>] Customer(customerId:CustomerId,firstname:string,lastname:string,version:int)=
    new()=Customer(CustomerId 0,"","",0)
    member val CustomerId  =customerId with get, set
    member val Firstname =firstname with get, set
    member val Lastname =lastname with get, set
    member val Orders = List<Order>() with get, set
    member val Version = version with get, set
and [<AllowNullLiteral>] Product(productId:ProductId,cost:float,productName:string,version:int)=
    new()=Product(ProductId 0,0.0,"",0)
    member val ProductId =productId with get, set
    member val Cost =cost with get, set
    member val ProductName = productName with get, set
    member val Orders = List<ProductOrder>() with get, set
    member val Version = version with get, set
