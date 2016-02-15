
namespace TestFSharps

open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestOptional()=

    [<Test>]
    member this.TestMap()=
        let fool input = (box input).ToString()

        Some(1) |> Option.map fool |> should equal (Some("1"))
        None |> Option.map fool |> should equal None

    /// option value can be viewed as collection with zero or one value
    [<Test>]
    member this.TestBind()=
        let fool x =
            if x < 0 then None
            else Some(string x)

        [Some(1);Some(-1);None] 
            |> List.map (Option.bind fool)
            |> should equal [Some("1");None;None]
        
        
    [<Test>]
    member this.TestIter() =
        
        let fool x =
            let flag = ref false
            x |> Option.iter (fun _ -> flag := true)
            !flag

        Some(1) |> fool |> should be True
        None |> fool |> should be False
