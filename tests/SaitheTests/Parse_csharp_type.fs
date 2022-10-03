namespace Tests.Parse_csharp_type
open System.Runtime.Serialization
open System.Text.Json
open Xunit
open System
open CSharpTypes 
open Saithe
open System.ComponentModel
[<Serializable>]
[<CLIMutable>]
type PValueContainer={ V:ProductId; }


type ``Parse type``() = 

    [<Fact>]
    member this.SystemTextJson()=
        let data = @"{""V"":""ProductId/1""}"
        let result = JsonSerializer.Deserialize<PValueContainer>(data);
        let expected = { V = ProductId(1) }
        Assert.Equal(expected, result)

    [<Fact>]
    member this.SystemTextJson_serialize()=
        let expected = @"{""V"":""ProductId/1""}"
        let result = JsonSerializer.Serialize({ V = ProductId(1)})
        Assert.Equal(expected, result)

    [<Fact>]
    member this.SystemTextJson_deserialize_invalid_data()=
        let data = @"{""V"":""1""}"
        let ex = Assert.Throws<Exception>( fun ()->
           JsonSerializer.Deserialize<PValueContainer>(data) |> ignore
        )
        Assert.Equal ("Could not parse product id", ex.Message)

