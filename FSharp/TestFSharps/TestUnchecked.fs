
namespace TestFSharps

open System
open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestUnchecked() =

    [<Test>]
    member this.SimpleDemo() =
        Unchecked.compare 8 9 |> should be (lessThan 0)

        // For Unchecked.equals and Unchecked.hash, 
        // they are semantically similar to converting to the type “obj” 
        // and doing equality over that static type
        let f1 = fun x -> x + 1
        let f2 = fun x -> x + 1
        Unchecked.equals f1 f2 |> should be False

        let o1 = new obj()
        let o2 = new obj()

        // !!! below line cannot pass compilation, because obj doesn't implement IComparable
        // compare o1 o2
        // !!! since Unchecked module doesn't force static-time checking
        // !!! so below codes can compile successfully
        // !!! however, it will cause runtime error, since obj is not "comparable"
        (fun() -> Unchecked.compare o1 o2 |> ignore) |> should throw typeof<ArgumentException>