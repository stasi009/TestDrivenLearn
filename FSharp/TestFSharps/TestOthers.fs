
#light

namespace TestFSharps

open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestOthers()=
    [<Test>]
    member this.TestMultipleAssign() =
        let x,y = 1,2
        x |> should equal 1
        y |> should equal 2

        let a,b = 100,"hello"
        a |> should equal 100
        b |> should equal "hello"

    // chekanote: at least in member method, you can rebound the same "name" to another value, even different types
    [<Test>]
    member this.TestOutscope() =
        // chekanote: this "rebind" is called "outscope"
        // Outscoping a value doesn’t change the original value; 
        // it just means the name of the value is no longer accessible from the current scope.
        let x = 1 // "1" is outscoped
        let x = "hello wsu"

        x.GetType() |> should equal typeof<string>

    [<Test>]
    member this.TestAssignOperator() =
        // chekanote: "=" only represents "assignment" when it works with "let"
        let x = 5
        // chekanote: otherwise, it represents "equality comparison" 
        let y = x = 0
        y |> should be False

    // demonstrate the usage of "||>"
    [<Test>]
    member this.TestTwoArgPipeline() =

        let toUpper (s1:string) (s2:string) =
            s1.ToUpper(),s2.ToUpper()

        let concat s1 s2 = 
            sprintf "%s.%s" s1 s2

        ("stasi","cheka")
        ||> toUpper
        ||> concat
        |> should equal "STASI.CHEKA"
        