namespace CoreFs
open System
open System.Collections.Generic
open System.ComponentModel
open System.ComponentModel.DataAnnotations
open System.Text.Json.Serialization
open FSharpPlus
open Saithe
open Saithe.SystemTextJson

[<JsonConverter(typeof<ParseTypeJsonConverter<OrderId>>);
  TypeConverter(typeof<ParseTypeConverter<OrderId>>)>]
type OrderId = | OrderId of int
with
    member self.Value = match self with OrderId v -> v
    override this.ToString() = $"order-%i{this.Value}"
    static member TryParse value = trySscanf "order-%i" value |> map OrderId
    static member Parse value = match OrderId.TryParse value with | Some v -> v | _ -> raise ( ArgumentException "Failed to parse" )

[<JsonConverter(typeof<ParseTypeJsonConverter<ProductId>>);
  TypeConverter(typeof<ParseTypeConverter<ProductId>>)>]
type ProductId = | ProductId of int
with
    member self.Value = match self with ProductId v -> v
    override this.ToString() = $"product-%i{this.Value}"
    static member TryParse value = trySscanf "product-%i" value |> map ProductId
    static member Parse value = match ProductId.TryParse value with | Some v -> v | _ -> raise ( ArgumentException "Failed to parse" )
[<JsonConverter(typeof<ParseTypeJsonConverter<CustomerId>>);
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
[<CLIMutable>]
type ProductOrder = { Order:Order; Product:Product }
with
    static member Create (order,product) = { Order = order; Product = product }
and [<CLIMutable>] Order = {
    [<JsonPropertyName("id"); Key>]
    OrderId:OrderId
    OrderDate:DateTime
    Customer:Customer
    [<JsonIgnore>]
    Version:int
    [<JsonIgnore>]
    Products: Product seq
}
with
    static member Create orderDate customer = {
        OrderId = OrderId 0
        OrderDate = orderDate
        Version = 0
        Customer = customer
        Products = List<_>() }
and[<CLIMutable>] Customer = {
    [<JsonPropertyName("id"); Key>]
    CustomerId:CustomerId
    mutable Firstname:string
    mutable Lastname:string
    [<JsonIgnore>]
    Version:int
    [<JsonIgnore>]
    Orders: Order seq
}
with
    static member Default = {
        CustomerId = CustomerId 0
        Firstname = ""
        Lastname = ""
        Version = 0
        Orders = List<_>() }
and [<CLIMutable>] Product = {
    [<JsonPropertyName("id"); Key>]
    ProductId: ProductId
    mutable Cost: float
    [<JsonPropertyName("name")>]
    mutable ProductName: string
    Version: int
    [<JsonIgnore>]
    Orders: Order seq
}
with
    static member Default = {
        ProductId = ProductId 0
        Cost = 0
        ProductName = ""
        Version = 0
        Orders = List<_>() }
