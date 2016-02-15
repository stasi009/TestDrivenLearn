namespace TestFSharps

open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestRefCalls() = 
    
    (*
    Mutable variables are somewhat limited: 
    mutables are inaccessible outside of the scope of the function where they are defined. 
    Specifically, this means its not possible to reference a mutable in a subfunction of another function
    Ref cells get around some of the limitations of mutables
    *)
    [<Test>]
    member this.TestMutableLimitation() =
        // mutable can be used in ordinary loop and can be changed inside the loop
        let mutable result = 0
        for e in [1..3] do
            result <- result + e
        result |> should equal 6

        // however, mutable cannot be used in seq{}
        (* below codes cannot compile
            let scanned = seq {
                let mutable currentsum = 0
                yield currentsum

                for e in [|1..3|] do
                    currentsum <- currentsum + e
                    yield currentsum
            }
            scanned |> List.ofSeq |> ignore
        *)
        

    [<Test>]
    member this.TestPointerFeature() = 
        // ------------------------ both pointer points to same space
        let pointerX = ref 88
        let pointerY = pointerX
        pointerY |> should be (sameAs pointerX)
        // note: "refcell.value <- NewValue" is just the same as "refcell := NewValue"
        pointerX.Value <- !pointerX + 2
        !pointerX |> should equal 90
        !pointerY |> should equal 90
        // ------------------------ !!!!! note: always the same
        pointerY := 0 // := is just a overloaded operator for 'x.Value <- newvalue' 
        pointerY |> should be (sameAs pointerX)
        pointerY.Value <- !pointerY + 10
        !pointerX |> should equal 10
        !pointerY |> should equal 10
    
    [<Test>]
    member this.TestProperty() = 
        let x : int ref = ref 10 // demonstrate how to declare type explicitly
        x.Value |> should equal 10
        x.contents |> should equal 10
        x.Value <- 8 // it is editable
        x.contents |> should equal 8
        x.contents <- 9
        x.Value |> should equal 9
    
    [<Test>]
    member this.TestAsByRefParameter() = 
        let x = ref 8
        // Func4Test.increment &x.Value // !!!!!!!!!! cannot pass the compilation
        Func4Test.increment &x.contents
        !x |> should equal 9
    
    [<Test>]
    member this.TestMakingSideEffect() = 
        let seed = ref 0
        
        let counter() = 
            seed := !seed + 1
            !seed
        counter() |> should equal 1
        counter() |> should equal 2
        counter() |> should equal 3
    
    /// with reference cell, we can implement a simple object
    /// with pure functional technology
    [<Test>]
    member this.TestSimpleObjPattern() = 
        let factory initial = 
            // current state can be stored, but always be hided
            // we can manipulate this state, only be two API exposed
            // we can only add and retrieve current, but have no method
            // to do any other operation
            let state = ref initial
            let increase() = state := !state + 1
            let current() = !state
            increase, current
        
        let adder1, getter1 = factory 10
        getter1() |> should equal 10
        for index = 1 to 10 do
            adder1()
        getter1() |> should equal 20
        // ---------------------- create another "simple object"
        let adder2, getter2 = factory 61
        for index = 1 to 5 do
            adder1()
            adder2()
        getter1() |> should equal 25
        getter2() |> should equal 66
