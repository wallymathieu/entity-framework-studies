namespace CoreFs
open System
open System.Collections.Generic
open System.Text.Json.Serialization
open Saithe

type ValueTypeJsonConverter<'T when 'T :> obj>() =
  inherit JsonConverter<'T>()
  let mapping = ValueTypeMapping<'T>()
  let t = typeof<'T>
  override this.CanConvert(objectType) =
    objectType = t
  override this.Read(reader, objectType, _) : 'T =
    if (objectType = t) then
      let v = reader.GetString ()
      mapping.Parse(v) :?> 'T
    else if (Nullable.GetUnderlyingType(objectType) = t) then
      let v = reader.GetString ()
      if isNull v then
        Unchecked.defaultof<'T>
      else
        mapping.Parse(v) :?> 'T
    else failwithf $"Cant handle type %s{objectType.Name}, expects %s{t.Name} (1)"

  override this.Write(writer, value, _) =
    writer.WriteStringValue(string <| mapping.ToRaw(value))

open FSharpPlus

[<JsonConverter(typeof<ValueTypeJsonConverter<OrderId>>)>]
type OrderId (value:int) =
    member _.Value = value
    override this.ToString() = $"order-%i{value}"
    static member TryParse value = trySscanf "order-%i" value |> map OrderId
    static member Parse value = match OrderId.TryParse value with | Some v -> v | _ -> raise ( ArgumentException "Failed to parse" )

[<JsonConverter(typeof<ValueTypeJsonConverter<ProductId>>)>]
type ProductId (value:int) =
    member _.Value = value
    override this.ToString() = $"product-%i{value}"
    static member TryParse value = trySscanf "product-%i" value |> map ProductId
    static member Parse value = match ProductId.TryParse value with | Some v -> v | _ -> raise ( ArgumentException "Failed to parse" )
[<JsonConverter(typeof<ValueTypeJsonConverter<CustomerId>>)>]
type CustomerId (value:int) =
    member _.Value = value
    override this.ToString() = $"customer-%i{value}"
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
