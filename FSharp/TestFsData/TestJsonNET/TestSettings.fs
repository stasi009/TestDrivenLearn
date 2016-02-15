
namespace TestJson

open System
open Newtonsoft.Json
open Newtonsoft.Json.Converters

module TestSettings =

    type Person() =
        let mutable m_id = 0
        let mutable m_name : string = null

        member this.Id 
            with get() = m_id
            and set(newvalue) = m_id <- newvalue

        member this.Name 
            with get() = m_name
            and set(newvalue) = m_name <- newvalue

        override this.ToString() =
            sprintf "%d-%s" m_id m_name

    let private testNullValue() = 
        let p = new Person()

        // ------------------ include null (default)
        let setting = new JsonSerializerSettings()
        let includeNullText = JsonConvert.SerializeObject(p,setting)
        printfn "############# include null value:\n%s" includeNullText

        // ------------------ ignore null
        setting.NullValueHandling <- NullValueHandling.Ignore
        let ignoreNullText = JsonConvert.SerializeObject(p,setting)
        printfn "############# ignore null value:\n%s" ignoreNullText

    let private testDateTime() =

        let dt = DateTime.Now

        // ----------------------- ISO format
        let isoTxt = dt |> JsonConvert.SerializeObject
        printfn "ISO DateTime Format:\n\t%s" isoTxt

        // ----------------------- Javascript DateTime
        let jsTxt = JsonConvert.SerializeObject(dt,(new JavaScriptDateTimeConverter()))
        printfn "Javascript DateTime Format:\n\t%s" jsTxt

    let private testFormatting() =
        let p = new Person(Id=9,Name="stasi")

        // Formatting.None is the default option
        p
        |> JsonConvert.SerializeObject
        |> printfn "########## default settings (no indented), serialized to:\n%s" 

        JsonConvert.SerializeObject(p,Formatting.Indented)
        |> printfn "########## indented, serialized to:\n%s" 

    let main() =
        // testNullValue()
        // testDateTime()
        testFormatting()
        

