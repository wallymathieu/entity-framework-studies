namespace Tests.Handle_csharp_types

open System.Text.Json
open CSharpTypes 
open Xunit
open System

[<Serializable>]
[<CLIMutable>]
type Customer = 
  { Name : string
    Id : CustomerId }

[<Serializable>]
[<CLIMutable>]
type CustomerOrder = 
  { ProductDescription : string
    CustomerId : Nullable<CustomerId> 
    ProductId : Nullable<ProductId>
    OrderId : Nullable<OrderId>
  }

type ``Serialize and deserialize struct type``() = 
  
  [<Fact>]
  member this.Struct_SystemTextJson() = 
    let data = @"{""Name"":""Ctr"",""Id"":1}"
    let result = JsonSerializer.Deserialize<Customer>(data)
    Assert.Equal("Ctr", result.Name)
    Assert.Equal(1L, result.Id.Value)
  
  [<Fact>]
  member this.Struct_SystemTextJson_serialize() = 
    let expected = @"{""Name"":""Mgr"",""Id"":1}"
    let result = 
      JsonSerializer.Serialize({ Name = "Mgr"
                                 Id = CustomerId(1) })
    Assert.Equal(expected, result)

  //[<Fact>]
  member this.Struct_SystemTextJson_Nullable() = 
    let data = @"{""ProductDescription"":""Description"",""CustomerId"":1,""ProductId"":""ProductId/2"",""OrderId"":3}"
    let result = JsonSerializer.Deserialize<CustomerOrder>(data)
    Assert.Equal("Description", result.ProductDescription)
    Assert.Equal(1L, result.CustomerId.Value.Value)
    Assert.Equal(2L, result.ProductId.Value.Value)
    Assert.Equal(3L, result.OrderId.Value.Value)

  [<Fact>]
  member this.Struct_SystemTextJson_Nullable_null() = 
    let data = @"{""ProductDescription"":""Description"",""CustomerId"":null,""ProductId"":null,""OrderId"":null}"
    let result = JsonSerializer.Deserialize<CustomerOrder>(data)
    Assert.Equal("Description", result.ProductDescription)
    Assert.Equal(false, result.CustomerId.HasValue)
    Assert.Equal(false, result.ProductId.HasValue)  
    Assert.Equal(false, result.OrderId.HasValue)  

  [<Fact>]
  member this.Struct_SystemTextJson_serialize_Nullable() = 
    let expected = @"{""ProductDescription"":""Description"",""CustomerId"":1,""ProductId"":""ProductId/2"",""OrderId"":""3""}"
    let result = 
      JsonSerializer.Serialize({ ProductDescription = "Description"
                                 CustomerId = Nullable<CustomerId>(CustomerId(1))
                                 ProductId= Nullable<ProductId>(ProductId(2))
                                 OrderId=Nullable<OrderId>(OrderId(3)) })
    Assert.Equal(expected, result)


  [<Fact>]
  member this.Struct_SystemTextJson_serialize_Nullable_null() = 
    let expected = @"{""ProductDescription"":""Description"",""CustomerId"":null,""ProductId"":null,""OrderId"":null}"
    let result = 
      JsonSerializer.Serialize({ ProductDescription = "Description"
                                 CustomerId = Nullable<CustomerId>()
                                 ProductId= Nullable<ProductId>()
                                 OrderId=Nullable<OrderId>() })
    Assert.Equal(expected, result)
