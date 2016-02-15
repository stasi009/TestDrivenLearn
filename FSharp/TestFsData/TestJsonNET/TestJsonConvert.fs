

namespace TestJson

open System
open Newtonsoft.Json

module TestJsonConvert =

    type Person = 
        {
            Id : int
            Name : string
        }
        override this.ToString() = sprintf "%d-%s" this.Id this.Name

    let private makePeople count =
        seq {
            for index = 1 to count do
                yield {Name = (sprintf "p%d" index);Id = index}
        } 

    let private testList() =
        // --------------- serialize
        let total = 3
        let people = makePeople total

        let text = JsonConvert.SerializeObject(people,Formatting.Indented)
        printfn "%s" text

        // --------------- deserialize
        let deserialized = JsonConvert.DeserializeObject<Person[]>(text)
        deserialized
        |> Seq.iteri (fun index p -> printfn "[%d] %s" (index+1) (p.ToString()))

    let main() =
        testList()
        

