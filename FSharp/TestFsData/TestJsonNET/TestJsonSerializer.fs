namespace TestJson

open System
open System.IO
open Newtonsoft.Json
open Newtonsoft.Json.Converters

module TestJsonSerializer = 
    // CANNOT be private type
    type Product() = 
        let mutable m_name = ""
        let mutable m_price = 0.0f
        let mutable m_expiry = DateTime.Now
        let mutable m_sizes : string [] = null
        
        member this.Name 
            with get () = m_name
            and set (newvalue) = m_name <- newvalue
        
        member this.Price 
            with get () = m_price
            and set (newvalue) = m_price <- newvalue
        
        member this.Expiry 
            with get () = m_expiry
            and set (newvalue) = m_expiry <- newvalue
        
        member this.Sizes 
            with get () = m_sizes
            and set (newvalue) = m_sizes <- newvalue
    
    let private tryDefaultWithConverter() = 
        let p = new Product()
        p.Name <- "iphone"
        p
        |> JsonConvert.SerializeObject
        |> printfn "%s"
    
    let private tryWithSerializer() = 
        let p = new Product(Name = "ms", Price = 88.66f)
        let serializer = new JsonSerializer()
        serializer.Converters.Add <| new JavaScriptDateTimeConverter()
        serializer.NullValueHandling <- NullValueHandling.Ignore
        use sw = new StringWriter()
        use jwriter = new JsonTextWriter(sw)
        serializer.Serialize(jwriter, p)
        printfn "%s" <| sw.ToString()
    
    let main() = 
        tryDefaultWithConverter()
        tryWithSerializer()
