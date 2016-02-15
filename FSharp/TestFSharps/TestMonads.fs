
namespace TestFSharps

open NUnit.Framework
open FsUnit

type ValueWrapper<'t> =
    | Value of 't

type ValueWrapperBuilder()=
    member this.Bind(Value(v),callback)= callback v
    member this.Return(v) = Value(v)

type OptionBuilder()=
    member this.Bind(opt,callback)=
        match opt with
            | Some(v) -> callback(v)
            | _ -> None // return directly

    member this.Return v = Some(v)

/// Computation Expression is just an implementation of Monad theory in F#
[<TestFixture>]
type TestMonads()=

    [<Test>]
    member this.TestSimple() = 
        let valuewrapper = new ValueWrapperBuilder()
        let (Value(embedded)) = valuewrapper {
            let! x = Value(1)
            let! y = Value("hello")
            return x+ y.Length
        }
        embedded |> should equal 6

    [<Test>]
    member this.TestOptionBuilder()=
        let option = new OptionBuilder()

        let fool optX optY = option {
            let! x = optX
            let! y = optY
            return x + y
        }

        [(Some(1),Some(2));(None,Some(100));(Some(2),None)]
            |> List.map (fun (optx,opty) -> fool optx opty)
            |> should equal [Some(3);None;None]
