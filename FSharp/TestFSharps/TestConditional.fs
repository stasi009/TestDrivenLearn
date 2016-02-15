
namespace TestFSharps

open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestConditional() =

    static member CompareAndPrint a b =
        if a < b then
            sprintf "%d < %d" a b
        elif a = b then
            sprintf "%d = %d" a b
        else
            sprintf "%d > %d" a b
    
    [<Test>]
    member this.TestReturnValue() =
        let a,b = 1,2

        let result = 
            if a < b then
                "a < b"
            else
                "a >= b"

        result |> should equal "a < b"

    [<Test>]
    member this.TestCompareAndPrint() = 
        (TestConditional.CompareAndPrint 1 2 ) |> should equal "1 < 2"
        (TestConditional.CompareAndPrint 4 3 ) |> should equal "4 > 3"
        (TestConditional.CompareAndPrint 9 9 ) |> should equal "9 = 9"
                    

