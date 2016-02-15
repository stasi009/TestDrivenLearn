
namespace TestFSharps

open NUnit.Framework
open FsUnit

// type abbrevation
type student = int * string

[<TestFixture>]
type TestTuples() = 

    [<Test>]
    member this.SimpleTest() = 
        let t : student = (1,"stasi")
        (fst t) |> should equal 1
        (snd t) |> should equal "stasi"

    [<Test>]
    member this.Decompose()=
        let id,name,score = (1,"cheka",100)
        id |> should equal 1
        name |> should equal "cheka"
        score |> should equal 100

    [<Test>]
    member this.TestAsArguments()=
        let addTwoArgs x y = x + y
        (addTwoArgs 1 2) |> should equal 3

        let addOneTuple (x,y) = x + y
        (addOneTuple (4,2)) |> should equal 6

    [<Test>]
    member this.TestMatch()=
        [("tom",1,88.8);("mary",2,59.9);("dick",3,61.2)]
            |> List.filter (fun item ->
                                match item with
                                    | (name,id,score)-> score > 60.0)
            |> List.map (fun item ->
                                match item with
                                    | (name,id,score)->name)
            |> should equal ["tom";"dick"]


