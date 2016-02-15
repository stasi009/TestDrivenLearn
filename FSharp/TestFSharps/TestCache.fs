
namespace TestFSharps

open System.Collections.Generic

open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestCache()=

    [<Test>]
    member this.TestPreCompute()=
        let hasWord (words: string seq) (counter : int ref)=
            // this "time-consuming" will occur only once 
            let table = words |> Set.ofSeq 
            incr counter
            fun word -> table.Contains word

        let counter = ref 0
        let hasAgency = hasWord ["stasi";"kgb";"cheka";"kgb";"cia";"mi5"] counter
        !counter |> should equal 1

        "stasi" |> hasAgency |> should be True
        "fbi" |> hasAgency |> should be False

        // side-effect only occur once
        !counter |> should equal 1

    static member private Memoize (f: 'input->int ref->'result)=
        let table = new Dictionary<'input,'result>()

        fun input sideEffect ->
            if table.ContainsKey input then
                table.[input]
            else 
                let result = f input sideEffect
                table.Add(input,result)
                result
    [<Test>]
    member this.TestMemoize()=
        let side_effect_counter (f: int->int ref->int) total=
            let counter = ref 0

            for index = 1 to total do
                f 100 counter |> ignore

            !counter

        let fool (num : int) (counter: int ref)=    
            incr counter
            num * num

        let memoized_fool = TestCache.Memoize fool

        side_effect_counter fool 3 |> should equal 3
        side_effect_counter memoized_fool 3 |> should equal 1 

    /// lazy has two featurs:
    /// 1. calculate the value when needed
    /// 2. calculate the value only once. 
    /// after the first calculation, it will remember the result for later reuse
    [<Test>]
    member this.TestLazy()=
        let sideEffect = ref 0

        let lazyed = lazy(
            incr sideEffect
            100
        )

        !sideEffect |> should equal 0

        // -------- two ways to get the value
        lazyed.Value |> should equal 100
        lazyed.Force() |> should equal 100
        lazyed.Value |> ignore

        !sideEffect |> should equal 1 // side effect only happen once

        
            
