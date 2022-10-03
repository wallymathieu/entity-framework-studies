namespace CoreFs
open System
open System.Collections.Generic
open System.ComponentModel
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema
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
    Customer:Customer option
    [<JsonIgnore>]
    Version:int
    [<JsonIgnore>]
    Products: List<ProductOrder>
}
with
    [<JsonPropertyName("products");NotMapped>]
    member this.OrderProducts = this.Products |> ResizeArray.map (fun op->op.Product)
    static member Default = {
        OrderId = OrderId 0
        OrderDate = DateTime.MinValue
        Version = 0
        Customer = None
        Products = List<_>() }
and[<CLIMutable>] Customer = {
    [<JsonPropertyName("id"); Key>]
    CustomerId:CustomerId
    Firstname:string
    Lastname:string
    [<JsonIgnore>]
    Version:int
    [<JsonIgnore>]
    Orders: List<Order>
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
    ProductId:ProductId
    Cost:float
    ProductName:string
    Version:int
    [<JsonIgnore>]
    Orders: List<ProductOrder>
}
with
    static member Default = {
        ProductId = ProductId 0
        Cost = 0
        ProductName = ""
        Version = 0
        Orders = List<_>() }
