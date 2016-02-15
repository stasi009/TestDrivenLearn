
namespace TestFSharps

open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestDeferFeature()=
    [<Test>]
    member this.TestDelaySideEffect() =
        let make_sequence (sideEffect:int array) =
            sideEffect.[0] <- sideEffect.[0] + 1
            seq {1..3}

        // -------------------- active sequence
        // for active sequence, which is created without "Delay", the side effect happens when that sequence is built
        // even before that sequence is consumed, and that side effect happens only once
        // when consuming that sequence later on, that side effect won't happen again
        let sideEffect_whenCreate = [|0|]
        let active_seq = make_sequence sideEffect_whenCreate
        sideEffect_whenCreate.[0] |> should equal 1
        
        active_seq |> Seq.toArray |> ignore
        sideEffect_whenCreate.[0] |> should equal 1

        // -------------------- lazy sequence
        // for lazy sequence, sequence built with "Delay", that side effect won't happen until the sequence is consumed
        // and that side effect occurs everytime that sequence is consumed
        let sideEffect_whenUse = [|0|]
        let lazy_seq = Seq.delay( fun()-> make_sequence sideEffect_whenUse)
        sideEffect_whenUse.[0] |> should equal 0

        (Seq.toList lazy_seq) |> ignore
        sideEffect_whenUse.[0] |> should equal 1

        let temp = Seq.toArray lazy_seq
        sideEffect_whenUse.[0] |> should equal 2

    [<Test>]
    member this.TestReevaluateFeature()=
        let sideEffect = ref 0
        let numbers = seq {
            incr sideEffect
            yield 1
            yield 2
        }
        !sideEffect |> should equal 0

        numbers |> should equal [1;2]
        !sideEffect |> should equal 1

        for index = 1 to 3 do
            numbers |> List.ofSeq |> ignore
        !sideEffect |> should equal 4

    [<Test>]
    member this.TestReevaluateFeature2()=
        let environment = ref 0

        let numbers = seq {
            for index = 1 to !environment do
                yield index
        }

        environment := 3
        numbers |> should equal [1;2;3]

        environment := 6
        numbers |> should equal [1;2;3;4;5;6]

    /// this test demonstrate that delayed-sequence will be built only when it is used
    /// so at that time, it will use the latest external enviroment
    [<Test>]
    member this.TestDelayLatestEnvironment_withFactoryFunc() =
        let environment = [|0|]

        let make_sequence() = seq {1..environment.[0]}

        // ------------------- Non-Delay Sequence
        environment.[0] <- 2
        let nonDelaySeq = make_sequence()
        nonDelaySeq |> should equal [1;2]

        // the external environment is changed, but the sequence has already fixed
        environment.[0] <- 100
        nonDelaySeq |> should equal [1;2]

        // ------------------- Delay Sequence
        let delayedSeq = Seq.delay make_sequence

        environment.[0] <- 2
        delayedSeq |> should equal [1;2]

        // the changes in the external environment take effects
        environment.[0] <- 4
        delayedSeq |> should equal [1;2;3;4]

    [<Test>]
    member this.TestDelayLatestEnvironment_withSeqItself() =
        // -------------- sequence fixed when generated
        let environment = ref 0
        let fixedNumbers = seq {1..!environment}

        fixedNumbers |> should equal []
        
        environment := 300
        fixedNumbers |> should equal []

        // -------------- sequence re-evaluated every time being used
        let deferred = Seq.delay (fun()->seq {1..!environment})

        environment := 3
        deferred |> should equal [1;2;3]

        environment := 6
        deferred |> should equal [1;2;3;4;5;6]

    [<Test>]
    member this.TestDeferredFeature()=
        let resizearray = new ResizeArray<int>([1;2])
        let numbers = seq {for item in resizearray -> item}
        numbers |> should equal [1;2]

        resizearray.Add 300
        numbers |> should equal [1;2;300]
