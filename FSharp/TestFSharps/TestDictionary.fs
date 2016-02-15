
namespace TestFSharps

open System.Collections.Generic

open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestDictionary()=

    [<Test>]
    member this.Sample()=
        // by passing in "HashIdentity.Structural", in some cases, performance may be improved
        let dict = new Dictionary<int,string>(HashIdentity.Structural)

        [(1,"cheka");(100,"stasi");(60,"kgb")]
            |> List.iter (fun (id,name)-> dict.Add(id,name))

        dict 
            |> Seq.map (fun kv->sprintf "%d-%s" kv.Key kv.Value)
            |> should equal ["1-cheka";"100-stasi";"60-kgb"]

        // ------------ access by key
        dict.Count |> should equal 3
        dict.[3] <- "gru"
        dict.Count |> should equal 4

        dict.[1] |> should equal "cheka"
        dict.[1] <- "mss"
        dict.[1] |> should equal "mss"

        // ------------ TryGetValue's result is wrapped into tuple
        let found = dict.TryGetValue 100
        fst found |> should be True
        snd found |> should equal "stasi"

        let notfound = dict.TryGetValue 111
        fst notfound |> should be False

    [<Test>]
    member this.TestCompoundKey()=
        let dict = new Dictionary<int*int,string>()
        dict.Add((1,1),"stasi")
        dict.Add((2,2),"kgb")

        dict.[(2,2)] |> should equal "kgb"

    /// use "dict" to generate a read-only function
    [<Test>]
    member this.TestReadonlyByDict()=
        let dictionary = dict [("stasi",1);("kgb",2)]
        dictionary.IsReadOnly |> should be True

        (fun()-> dictionary.Add("cheka",300)) |> should throw typeof<System.NotSupportedException>

        dictionary.ContainsKey "stasi" |> should be True

        seq {
            for kv in dictionary do
                yield kv.Value
        } |> should equal [1;2]

    [<Test>]
    member this.TestKeyNotFoundException()=
        let dictionary = dict [("stasi",1);("kgb",2)]
        (fun()-> dictionary.["NotExisted"] |> ignore) |> should throw typeof<KeyNotFoundException>
        