
namespace TestFSharps

open NUnit.Framework
open FsUnit

module Module4Delegates =
    type TupleDelegate = delegate of (int*int)->int // accept a tuple and return a int
    type MultiParmDelegate = delegate of int*int->int // accept two ints and return a int
open Module4Delegates

[<TestFixture>]
type TestDelegate() = 

    [<Test>]
    member this.TestTupleDelegate()=
        let add = new TupleDelegate(fun (a,b)->a+b)
        add.Invoke (1,2) |> should equal 3

        let multiple = new TupleDelegate(fun (a,b) -> a * b)
        multiple.Invoke (3,4) |> should equal 12

        let add_ten other = add.Invoke (10,other)
        add_ten 5 |> should equal 15

    [<Test>]
    member this.TestMultiParmDelegate()=
        let add = new MultiParmDelegate(fun a b -> a + b)
        add.Invoke (1,2) |> should equal 3

        let multiple = new MultiParmDelegate(fun a b -> a * b)
        multiple.Invoke (3,4) |> should equal 12

        let add_ten other = add.Invoke (10,other)
        add_ten 6 |> should equal 16



