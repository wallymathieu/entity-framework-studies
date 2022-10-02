namespace Tests.Serialize_and_deserialize_type
open System.Text.Json
open System.Text.Json.Serialization
open Xunit
open System
open Saithe
open SaitheSystemTextJson
open System.ComponentModel

[<JsonConverter(typeof<ValueTypeStringSystemTextJsonConverter<ValueType>>)>]
type ValueType =
  { Value : string }

[<JsonConverter(typeof<ValueTypeIntSystemTextJsonConverter<IntValueType>>)>]
type IntValueType =
  { Value : int }

[<JsonConverter(typeof<ValueTypeStringSystemTextJsonConverter<CSharpyValueType>>)>]
type CSharpyValueType(value : string) =
  member this.Value = value

[<JsonConverter(typeof<ValueTypeIntSystemTextJsonConverter<CSharpyIntValueType>>)>]
type CSharpyIntValueType(value : int) = 
  member this.Value = value

[<Serializable>]
[<CLIMutable>]
type ValueContainer = 
  { Value : ValueType
    IntValue : IntValueType }

[<Serializable>]
type CSharpyValueContainer(value : CSharpyValueType, intValue : CSharpyIntValueType) = 
  member val Value = value with get, set
  member val IntValue = intValue with get, set

type ``Serialize and deserialize type``() = 
  
  [<Fact>]
  member this.SystemTextJson() = 
    let data = @"{""Value"":""Ctr"",""IntValue"":1}"
    let result = JsonSerializer.Deserialize<ValueContainer>(data)
    
    let expected = 
      { Value = { Value = "Ctr" }
        IntValue = { Value = 1 } }
    Assert.Equal(expected, result)
  
  [<Fact>]
  member this.SystemTextJson_serialize() = 
    let expected = @"{""Value"":""Mgr"",""IntValue"":1}"
    
    let result = 
      JsonSerializer.Serialize({ Value = { Value = "Mgr" }
                                 IntValue = { Value = 1 } })
    Assert.Equal(expected, result)
  
  [<Fact>]
  member this.CSharp_SystemTextJson() = 
    let data = @"{""Value"":""Ctr"",""IntValue"":1}"
    let result = JsonSerializer.Deserialize<CSharpyValueContainer>(data)
    Assert.Equal("Ctr", result.Value.Value)
    Assert.Equal(1, result.IntValue.Value)
  
  [<Fact>]
  member this.CSharp_SystemTextJson_serialize() = 
    let expected = @"{""Value"":""Mgr"",""IntValue"":1}"
    let result = JsonSerializer.Serialize(CSharpyValueContainer(CSharpyValueType("Mgr"), CSharpyIntValueType(1)))
    Assert.Equal(expected, result)
