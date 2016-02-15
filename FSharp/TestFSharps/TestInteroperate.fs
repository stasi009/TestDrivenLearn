
#light

namespace TestFSharps

open System.Collections.Generic
open System.Text

open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestInteroperate()=
    
    [<Test>]
    member this.TestResizeArray()=
        // ResizeArray is just an abbreviation for List<T>
        let strArrayList = new ResizeArray<string[]>()
        strArrayList.Add([|"hello";"wsu"|])
        strArrayList.Add([|"from";"F#"|])

        strArrayList 
            |> Seq.concat
            |> should equal ["hello";"wsu";"from";"F#"]

        // ------------- initialize with sequence
        let intlist = new ResizeArray<int>(seq{for index in 1..3->index * index})
        intlist.[1] |> should equal 4
        intlist |> should equal [1;4;9]

    [<Test>]
    member this.TestForEach()=
        let myconcat (sequence : string seq)=
            let sb = new System.Text.StringBuilder()

            for item in sequence do
                sb.Append item |> ignore

            sb.ToString()

        ["hello";" f#"] |> myconcat |> should equal "hello f#"
        [|"stasi ";"kgb"|] |> myconcat |> should equal "stasi kgb"

    /// mutable .NET classeses can be captured and used in the closure
    [<Test>]
    member this.TestMutableCaptured() =
        let numbers = {1..4}

        let sb = new StringBuilder()
        numbers |> Seq.iter (fun item-> sb.Append(string item) |> ignore)
        sb.ToString() |> should equal "1234"

        let alist = new List<int>()
        numbers |> Seq.iter (fun item->alist.Add(item))
        alist |> should equal [1;2;3;4]

    /// demonstrate both ways to call functions with "out argument"
    [<Test>]
    member this.TestFuncWithOutArgs()=
        // -------------- get out argument by "byref + &"
        let mutable result1 = -1
        let success = System.Int32.TryParse("101",&result1)
        success |> should be True
        result1 |> should equal 101

        // -------------- wrap out argument into tuple
        let result2 = System.Int32.TryParse("12")
        fst result2 |> should be True
        snd result2 |> should equal 12

        let success,_ = System.Int32.TryParse("none")
        success |> should be False

    [<Test>]
    member this.TestByRefArgument()=
        // ------------ use multiple and &
        let mutable result1 = 0
        let success = System.Int32.TryParse("999",&result1)
        result1 |> should equal 999

        // ------------ use ref
        let result2 = ref 0
        let success = System.Int32.TryParse("88",result2)
        !result2 |> should equal 88




