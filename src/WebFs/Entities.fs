namespace CoreFs
open System
open System.Collections.Generic
open System.ComponentModel
open System.ComponentModel.DataAnnotations.Schema
open System.Text.Json.Serialization
open FSharpPlus
open Saithe
open SaitheSystemTextJson

[<JsonConverter(typeof<ParseTypeSystemTextJsonConverter<OrderId>>);
  TypeConverter(typeof<ParseTypeConverter<OrderId>>)>]
type OrderId = | OrderId of int
with
    member self.Value = match self with OrderId v -> v
    override this.ToString() = $"order-%i{this.Value}"
    static member TryParse value = trySscanf "order-%i" value |> map OrderId
    static member Parse value = match OrderId.TryParse value with | Some v -> v | _ -> raise ( ArgumentException "Failed to parse" )

[<JsonConverter(typeof<ParseTypeSystemTextJsonConverter<ProductId>>);
  TypeConverter(typeof<ParseTypeConverter<ProductId>>)>]
type ProductId = | ProductId of int
with
    member self.Value = match self with ProductId v -> v
    override this.ToString() = $"product-%i{this.Value}"
    static member TryParse value = trySscanf "product-%i" value |> map ProductId
    static member Parse value = match ProductId.TryParse value with | Some v -> v | _ -> raise ( ArgumentException "Failed to parse" )
[<JsonConverter(typeof<ParseTypeSystemTextJsonConverter<CustomerId>>);
  TypeConverter(typeof<ParseTypeConverter<CustomerId>>)>]
type CustomerId = | CustomerId of int
with
    member self.Value = match self with CustomerId v -> v
    override this.ToString() = $"customer-%i{this.Value}"
    static member TryParse value = trySscanf "customer-%i" value |> map CustomerId
    static member Parse value = match CustomerId.TryParse value with | Some v -> v | _ -> raise ( ArgumentException "Failed to parse" )
module OrderId =begin let unwrap (id: OrderId) = id.Value end
module ProductId =begin let unwrap (id: ProductId) = id.Value end
module CustomerId =begin let unwrap (id: CustomerId) = id.Value end

[<AllowNullLiteral>]
type ProductOrder(order:Order,product:Product)=
    new()=ProductOrder(null, null)
    member val Order : Order=order with get, set
    //member val OrderId = if isNull order then OrderId 0 else order.OrderId with get, set
    member val Product : Product=product with get, set
    //member val ProductId = if isNull product then ProductId 0 else product.ProductId with get, set
and [<AllowNullLiteral>] Order(orderId:OrderId,orderDate:DateTime,customer:Customer,version:int) =
    new()=Order(OrderId 0,DateTime.MinValue,null,0)
    [<JsonPropertyName("id")>]
    member val OrderId =orderId with get, set
    member val OrderDate =orderDate with get, set
    member val Customer : Customer=customer with get, set
    [<JsonIgnore>]
    member val Products =List<ProductOrder>() with get, set
    [<JsonPropertyName("products");NotMapped>]
    member this.OrderProducts = this.Products |> ResizeArray.map (fun op->op.Product)
    [<JsonIgnore>]
    member val Version = version with get, set

and [<AllowNullLiteral>] Customer(customerId:CustomerId,firstname:string,lastname:string,version:int)=
    new()=Customer(CustomerId 0,"","",0)
    [<JsonPropertyName("id")>]
    member val CustomerId  =customerId with get, set
    member val Firstname =firstname with get, set
    member val Lastname =lastname with get, set
    [<JsonIgnore>]
    member val Orders = List<Order>() with get, set
    [<JsonIgnore>]
    member val Version = version with get, set
and [<AllowNullLiteral>] Product(productId:ProductId,cost:float,productName:string,version:int)=
    new()=Product(ProductId 0,0.0,"",0)
    [<JsonPropertyName("id")>]
    member val ProductId =productId with get, set
    member val Cost =cost with get, set
    member val ProductName = productName with get, set
    [<JsonIgnore>]
    member val Orders = List<ProductOrder>() with get, set
    [<JsonIgnore>]
    member val Version = version with get, set
