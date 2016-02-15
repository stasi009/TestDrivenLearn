
#light

namespace TestFSharps

open NUnit.Framework
open FsUnit

module Func4Test =
    /// functions which contain "byref" cannot be defined within another function
    /// it must be top-level functions defined within module or class
    let increment (num : int byref) =
        num <- num + 1

    let test_lazy() =
        let lazy_add x y = lazy (
            printfn "side effect must only happen once"
            x + y
        )
        
        let cached = lazy_add 1 2
        
        printfn "1st invocation, result=%d" (cached.Force())
        printfn "2nd invocation, result=%d" (cached.Force())


[<TestFixture>]
type TestFunctions() =

    [<Test>]
    member this.TestInferenceArgType() =
        let add x y = x + y

        // !!! chekanote: the principle here: when define function "add", the compiler cannot infer the argument's type
        // !!! it only infer its arguments' type when this function is first invoked
        // !!! from that moment on, the arguments' type are fixed, and cannot accept other types
        // (add 1 2 ) |> should equal 3
        (add "hello" " wsu") |> should equal "hello wsu"

        // -------------- explicitly specify the arguments' type
        let minus (x:double) (y:double) = x - y
        (minus 3.0 2.0) |> should equal 1.0    
        
    [<Test>]
    member this.TestByRef()=
        let mutable value = 1
        Func4Test.increment(&value)
        value |> should equal 2

    [<Test>]
    member this.TestPartialArguments()=
        let sumXY x y = x + y
        let sum10Y = sumXY 10
        (sum10Y 2) |> should equal 12
        (sum10Y 5) |> should equal 15

        let concatAB a b = sprintf "%s %s" a b
        let concatHelloB = concatAB "hello" 
        (concatHelloB "wsu") |> should equal "hello wsu" 
        (concatHelloB "F#") |> should equal "hello F#"

    [<Test>]
    member this.TestTupleArguments()=
        let addTwoArgs x y = x + y
        (addTwoArgs 1 2) |> should equal 3

        let addOneTuple (x,y) = x + y
        (addOneTuple (4,2)) |> should equal 6

    [<Test>]
    member this.TestFuncCannotOverload()=
        let add x y = x + y
        let add (x,y) = x + y
        
        // !!!!!!!! chekanote: when functions with the same name defined in the same scope
        // !!!!!!!! the later definition overwrites the previous one
        // !!!!!!!! so in this scope, there is only one definition available for "add", which accepts tuples
        // let result = add 1 2
        let result = add (1,2)
        result |> should equal 3

    [<Test>]
    member this.TestNestFuncImmutableCapture() =
        // 1. chekanote: nested function can access variables defined in the outer function
        // 2. chekanote: those external variables are fixed when that nested functions are defined
        // even though, that external variable may re-direct to others, but that won't change the nested function
        let prefix = "Hello"
        let concat x = sprintf "%s %s" prefix x

        (concat "wsu") |> should equal "Hello wsu"

        let prefix = "Good"
        let result = concat "F#"
        result |> should not' (equal "Good F#")
        result |> should equal "Hello F#"

    [<Test>]
    member this.TestNestFuncMutableCapture()=
        // ------------------ captured variable: immutable
        let immutableValue = 10
        let increment_immutable_capture y = immutableValue + y

        let immutableValue = 2
        (increment_immutable_capture 10) |> should equal 20

        // ------------------ captured variable: mutable
        (* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // chekanote: mutable symbol cannot be captured in closure
        let mutable mutableValue = 0
        let increment_mutable_capture y = mutableValue + y
        mutableValue <- 2
        *)

    [<Test>]
    member this.TestClosure() =
        // ---------------------------------------- sample1
        // closure are fixed when the function is defined
        let prefix = "hello"
        let greeting1 x = sprintf "%s %s" prefix x

        // outscope, or say, re-direct
        // reference to a new memory-address which is different from the closure
        // so it won't affect the result of the function
        // if you do want some side-effect, that is, chaning the outer captured variable
        // to affect the result of the function, we should use "reference cell" instead
        let prefix = ""
        "wsu" |> greeting1 |> should equal "hello wsu"

        // ---------------------------------------- sample2
        // chekanote: here the inner "prefix" overwrite the outer same-name "prefix"
        let factory prefix =
            let maker (name: string) = sprintf "%s %s" prefix name
            maker

        let greeting2 = factory "hi"
        "f#" |> greeting2 |> should equal "hi f#"

    [<Test>]
    member this.TestClosureScope() =
        let scope = "global"

        let fool() =
            let scope = "local"
            fun () -> scope 

        // demonstrate that closure is fixed when defined
        // not when invoked
        fool()() |> should equal "local"

    static member Handler (n:int) (f: int->int) =
        f(n)

    [<Test>]
    member this.TestHighOrderFunctions()=
        let square n = n * n
        let half n = n/ 2

        let funcFactory seed =
            let f x = x + seed
            f
        let addBySeed = funcFactory 8

        (TestFunctions.Handler 5 square) |> should equal 25
        (TestFunctions.Handler 5 half) |> should equal 2
        (TestFunctions.Handler 10 addBySeed) |> should equal 18

    [<Test>]
    member this.TestLambdaExpression()=
        let cube = fun n -> n * n * n
        (cube 4) |> should equal 64

        let square = fun (x:double) -> x * x
        (square 1.1) |> should (equalWithin 1e-6) 1.21

        (TestFunctions.Handler 5 (fun n-> n * n )) |> should equal 25
        (TestFunctions.Handler 5 (fun n-> n/2)) |> should equal 2

    [<Test>]
    member this.TestComposition() =
        let increment x = x + 1
        let square x = x * x

        // same as "square(increment(x))"
        let increment_square = increment >> square
        10 |> increment_square |> should equal 121

        // same as "increment(square(x))"
        let square_increment = square >> increment
        10 |> square_increment |> should equal 101

    [<Test>]
    member this.TestContinuation() =
        let mulWithCallback x y callback = 
            callback (x * y)

        (mulWithCallback 10 10 (fun n -> n+ 1)) |> should equal 101

    [<Test>]
    member this.TestLazy() =
        let calculate x y =
            lazy(
                let val1 = x + 1
                let val2 = y * y
                val1 - val2
            )

        let lazyResult = calculate 100 5

        lazyResult.Force() |> should equal 76

        // the second time, it will use that cached value, it won't repeat the calculation
        lazyResult.Force() |> should equal 76

    [<Test>]
    member this.TestExplicitSignature()=
        let fool (x:int) (y:int) : string =
            string (x+y)

        (fool 1 200) |> should equal "201"

    [<Test>]
    member this.TestCaptureVarInLoop()=
        // each loop variable is totally isolated, different functions will not share the same loop variable
        let functions = [|
            for index in 1..3 ->
                fun() -> index
        |]
        functions.[0]() |> should equal 1
        functions.[1]() |> should equal 2
        functions.[2]() |> should equal 3

    [<Test>]
    member this.TestPrivateFuncWhenAssignValue()=
        // define a private helper function when assigning a value
        let msg = 
            let prefix = "Mr. "
            let fool id name = sprintf "%d: %s%s" id prefix name
            fool 100 "cheka"
        msg |> should equal "100: Mr. cheka"


    




            

        




        
