
namespace TestFSharps

open System.Collections.Generic
open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestMaps()=

    [<Test>]
    member this.Sample()=
        let dict = new Map<string,int>( [("stasi",1);("kgb",2)] )

        dict.["stasi"] |> should equal 1
        (dict.ContainsKey "kgb") |> should be True
        dict |> Map.containsKey "kgb" |> should be True

        (dict.ContainsKey "NotExisted") |> should be False
        dict |> Map.containsKey "NotExisted" |> should be False

    [<Test>]
    member this.TestDuplicatedKeys() =
        // it won't throw exception, if the key is duplicated
        // the following value will just overwrite the previous value
        let dict = 
            [|("stasi",6);("stasi",9)|]
            |> Map.ofSeq

        dict.["stasi"] |> should equal 9
        
    [<Test>]
    member this.TestCompositeKey()=
        let dict = new Map<string*int,int>([(("hello",1),1);(("wsu",2),2)])
        dict.[("hello",1)] |> should equal 1

    [<Test>]
    member this.TestKeyNotFoundError() =
        let dict = new Map<string,int>( [("stasi",1);("kgb",2)] )
        (fun() -> dict.["KeyNotFound"] |> ignore) |> should throw typeof<KeyNotFoundException>

    /// modification on the sampe NOT happens on the map itself
    /// but generate and return a new map
    [<Test>]
    member this.TestImmutableFeature()=
        let orimap = [(1,"stasi");(100,"cheka")] |> Map.ofSeq
        let newmap = orimap.Add(3,"gru")

        obj.ReferenceEquals(orimap,newmap) |> should be False
        newmap 
            |> Seq.map (fun kv -> sprintf "%d-%s" kv.Key kv.Value)
            |> should equal ["1-stasi";"3-gru";"100-cheka"]

    [<Test>]
    member this.TestAdd()=
        let orimap = new Map<int,string>([(1,"stasi");])
        
        // ------------ add non-existing key
        let newmap = orimap.Add(100,"cheka")
        newmap.Count |> should equal 2
        orimap.Count |> should equal 1

        // ------------ add duplicate key
        // when adding duplicate key, no exception will be thrown
        // the returned new map will just contain the new KeyValuePair
        // just like overwrite
        let newmap2 = orimap.Add(1,"kgb")
        newmap2.Count |> should equal 1
        newmap2.[1] |> should equal "kgb"
        orimap.[1] |> should equal "stasi"

    [<Test>]
    member this.TestRemove()=
        let orimap = [(1,"stasi");(100,"cheka")] |> Map.ofSeq

        // ------------ remove existing key
        let newmap = orimap.Remove 1
        newmap.Count |> should equal 1
        newmap.[100] |> should equal "cheka"

        orimap.Count |> should equal 2 // original map not changed

        // ------------ remove non-existing key
        // if the key is not found, nothing happens, just return the same map as the input
        let newmap2 = orimap.Remove -1
        newmap2.Count |> should equal 2
        newmap2 |> should equal orimap


[<TestFixture>]
type TestSets()=

    [<Test>]
    member this.Sample()=
        [2;1;2;3;1;3]
            |> Set.ofList
            |> Seq.map (fun item->item)
            |> should equal [1;2;3]
