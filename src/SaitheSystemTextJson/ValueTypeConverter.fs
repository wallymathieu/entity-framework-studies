namespace SaitheSystemTextJson
open System
open System.Text.Json
open System.Text.Json.Serialization
open Saithe

[<AbstractClass>]
type ValueTypeSystemTextJsonConverter<'T when 'T :> obj>() =
  inherit JsonConverter<'T>()
  let mapping = ValueTypeMapping<'T>()
  let t = typeof<'T>
  abstract member Deserialize: byref<Utf8JsonReader> -> obj
  abstract member Serialize: Utf8JsonWriter*obj -> unit
  override this.CanConvert(objectType) =
    objectType = t
  override this.Read(reader, objectType, _) : 'T =
    if (objectType = t) then
      let v = this.Deserialize(&reader)
      mapping.Parse(v) :?> 'T
    else if (Nullable.GetUnderlyingType(objectType) = t) then
      let v = this.Deserialize(&reader)
      if isNull v then
        Unchecked.defaultof<'T>
      else
        mapping.Parse(v) :?> 'T
    else failwithf $"Cant handle type %s{objectType.Name}, expects %s{t.Name} (1)"

  override this.Write(writer, value, _) =
    this.Serialize ( writer, mapping.ToRaw(value))
type ValueTypeStringSystemTextJsonConverter<'T when 'T :> obj>() =
  inherit ValueTypeSystemTextJsonConverter<'T>()
  override this.Deserialize(r) = r.GetString() 
  override this.Serialize(w:Utf8JsonWriter,o:obj) = w.WriteStringValue(string o)
  
type ValueTypeIntSystemTextJsonConverter<'T when 'T :> obj>() =
  inherit ValueTypeSystemTextJsonConverter<'T>()
  override this.Deserialize(r) = r.GetInt32() 
  override this.Serialize(w:Utf8JsonWriter,o:obj) = w.WriteNumberValue( o :?> int32 )
type ValueTypeShortSystemTextJsonConverter<'T when 'T :> obj>() =
  inherit ValueTypeSystemTextJsonConverter<'T>()
  override this.Deserialize(r) = r.GetInt16() 
  override this.Serialize(w:Utf8JsonWriter,o:obj) = w.WriteNumberValue( o :?> int16 )

type ValueTypeLongSystemTextJsonConverter<'T when 'T :> obj>() =
  inherit ValueTypeSystemTextJsonConverter<'T>()
  override this.Deserialize(r) = r.GetInt64() 
  override this.Serialize(w:Utf8JsonWriter,o:obj) = w.WriteNumberValue( o :?> int64 )
  

