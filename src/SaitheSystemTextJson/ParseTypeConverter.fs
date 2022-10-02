namespace SaitheSystemTextJson
open System
open System.Reflection
open System.Text.Json.Serialization

type ParseTypeSystemTextJsonConverter<'T when 'T :> obj>() =
  inherit JsonConverter<'T>()
  let t = typeof<'T>
  let parse_method = t.GetTypeInfo().GetMethod("Parse")
  
  let parse s = 
    try 
      box (parse_method.Invoke(null, [| s |]))
    with :? TargetInvocationException as e -> raise (e.GetBaseException())

  override this.CanConvert(objectType) =
    objectType = t
  override this.Read(reader, objectType, _) : 'T =
    if (objectType = t) then
      let v = reader.GetString ()
      parse(v) :?> 'T
    else if (Nullable.GetUnderlyingType(objectType) = t) then
      let v = reader.GetString ()
      if isNull v then
        Unchecked.defaultof<'T>
      else
        parse(v) :?> 'T
    else failwithf $"Cant handle type %s{objectType.Name}, expects %s{t.Name} (1)"

  override this.Write(writer, value, _) =
    if isNull <| box value then
      writer.WriteNullValue()
    else
      writer.WriteStringValue(string value)
