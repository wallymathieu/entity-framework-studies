namespace Tests.Handle_structs

open System.Text.Json
open System.Text.Json.Serialization
open Xunit
open System
open Saithe
open SaitheSystemTextJson
open System.ComponentModel

[<TypeConverter(typeof<ValueTypeConverter<StructValueType>>)>]
[<JsonConverter(typeof<ValueTypeStringSystemTextJsonConverter<StructValueType>>)>]
type StructValueType = 
  struct
    val Value : string
  end
  with new(value:string)={ Value=value }

[<TypeConverter(typeof<ValueTypeConverter<StructIntValueType>>)>]
[<JsonConverter(typeof<ValueTypeIntSystemTextJsonConverter<StructIntValueType>>)>]
type StructIntValueType = 
  struct
    val Value : int
  end
  with new(value:int)={ Value=value }

[<Serializable>]
[<CLIMutable>]
type StructValueContainer = 
  { Value : StructValueType
    IntValue : StructIntValueType }

type ``Serialize and deserialize struct type``() = 
  
  [<Fact>]
  member this.Struct_SystemTextJson() = 
    let data = @"{""Value"":""Ctr"",""IntValue"":1}"
    let result = JsonSerializer.Deserialize<StructValueContainer>(data)
    Assert.Equal("Ctr", result.Value.Value)
    Assert.Equal(1, result.IntValue.Value)
  
  [<Fact>]
  member this.Struct_Newtonsoft_serialize() = 
    let expected = @"{""Value"":""Mgr"",""IntValue"":1}"
    
    let result = 
      JsonSerializer.Serialize({ Value = StructValueType("Mgr")
                                 IntValue = StructIntValueType(1) })
    Assert.Equal(expected, result)
