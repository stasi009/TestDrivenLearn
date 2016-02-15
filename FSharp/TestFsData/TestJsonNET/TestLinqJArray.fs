
namespace TestJson

open System
open Newtonsoft.Json
open Newtonsoft.Json.Linq

module TestLinqJArray = 

    let sample1() =
        let array = new JArray()

        [1;2;3]
        |> Seq.iter (fun n -> array.Add <| new JValue(n))

        printfn "%s" <| array.ToString()

    let main() =
        sample1()

