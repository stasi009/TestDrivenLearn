namespace TestJson

open System
open Newtonsoft.Json
open Newtonsoft.Json.Linq

module TrySamples = 
    // this class CANNOT be private
    // since Json.NET cannot process private types
    type Product = 
        { Name : string
          Expiry : DateTime
          Price : float32
          Sizes : string array }
    
    let private printProduct (p : Product) = printfn "Name=%s\nPrice=%f" p.Name p.Price
    
    let sample1() = 
        let product = 
            { Name = "Apple"
              Expiry = new DateTime(2012, 6, 1)
              Price = 3.14f
              Sizes = [| "Small"; "Medium"; "Large" |] }
        
        let text = JsonConvert.SerializeObject product
        printfn "serialized string:\n%s" text
        let deserialized = JsonConvert.DeserializeObject<Product>(text)
        printProduct deserialized
    
    let tryLinq2Json() = 
        let s = @"{
                    ""Name"": ""Apple"",
                    ""Expiry"": new Date(1230422400000),
                    ""Price"": 3.99,
                    ""Sizes"": [
                    ""Small"",
                    ""Medium"",
                    ""Large""
                    ]
                }"
        printfn "%s" s
        let o = JObject.Parse s
        let name = string o.["Name"]
        printfn "Name=%s" name
        let price = float32 o.["Price"]
        printfn "Price=%f" price
        // we cannot use 'JArray o.["Sizes"]' here
        // because that will create another JArray with one element
        // which becomes "[[...]]"
        let sizes = o.["Sizes"] :?> JArray
        printfn "Sizes="
        sizes |> Seq.iteri (fun index token -> printfn "[%d] %s" (index + 1) (string token))
    
    let main() = sample1()
// tryLinq2Json()
