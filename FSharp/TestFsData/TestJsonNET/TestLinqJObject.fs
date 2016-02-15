namespace TestJson

open System
open Newtonsoft.Json
open Newtonsoft.Json.Linq

module TestLinqJObject = 
    let private testJObject() = 
        let husband = new JObject()
        husband.Add <| new JProperty("Name", "stasi")
        husband.Add <| new JProperty("Age", 31)
        let wife = new JObject()
        wife.Add <| new JProperty("Name", "beauty")
        wife.Add <| new JProperty("Age", 24)
        husband.Add <| new JProperty("Wife", wife)
        printfn "%s" <| husband.ToString()
    
    let private jsonExample1 = 
        @"{""Name"" : ""Jack"", ""Age"" : 34, ""Colleagues"" : [{""Name"" : ""Tom"" , ""Age"":44},{""Name"" : ""Abel"",""Age"":29}] }"
    
    let private testParse() = 
        let o = JObject.Parse jsonExample1
        printfn "Name=%s,Age=%d" (string o.["Name"]) (int o.["Age"])
        printfn "Colleagues: "
        let colleagues = o.["Colleagues"].Children()
        colleagues
        |> Seq.map (fun token -> token.["Name"] |> string)
        |> Seq.iteri (fun index name -> printfn "[%d] %s" (index + 1) name)
    
    let private testModify() = 
        printfn "**************** original:\n%s" jsonExample1
        let o = JObject.Parse jsonExample1
        let colleagues = o.["Colleagues"] :?> JArray
        colleagues.[0].["Age"] <- new JValue(100)
        printfn "**************** modified:\n%s" <| o.ToString()
    
    let private testRemove() = 
        let o = JObject.Parse jsonExample1
        printfn "########### original:\n%s" <| o.ToString()
        // ------------ remove property
        o.Remove("Age") |> ignore
        printfn "########### property removed:\n%s" <| o.ToString()
        // ------------ remove child
        o.["Colleagues"].[0].Remove()
        printfn "########### child removed:\n%s" <| o.ToString()
    
    let private testSelectToken() = 
        let o = JObject.Parse jsonExample1
        let age = o.SelectToken("Colleagues[1].Age") |> int
        printfn "%d" age
    
    let private testSelectToken2() = 
        let o = JObject.Parse(@"{
                                  ""Stores"": [
                                    ""Lambton Quay"",
                                    ""Willis Street""
                                  ],
                                  ""Manufacturers"": [
                                    {
                                      ""Name"": ""Acme Co"",
                                      ""Products"": [
                                        {
                                          ""Name"": ""Anvil"",
                                          ""Price"": 50
                                        }
                                      ]
                                    },
                                    {
                                      ""Name"": ""Contoso"",
                                      ""Products"": [
                                        {
                                          ""Name"": ""Elbow Grease"",
                                          ""Price"": 99.95
                                        },
                                        {
                                          ""Name"": ""Headlight Fluid"",
                                          ""Price"": 4
                                        }
                                      ]
                                    }
                                  ]
                                }")
        [| "Manufacturers[0].Name"; "Manufacturers[0].Products[0].Price"; "Manufacturers[1].Products[0].Name" |]
        |> Seq.map (fun path -> 
               let token = o.SelectToken path
               (path, string token))
        |> Seq.iter (fun (path, s) -> printfn "%-36s: %s" path s)
    
    let main() = testJObject()
// testParse()
// testModify()
// testRemove()
// testSelectToken()
// testSelectToken2()
