namespace TestFSharps

open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestString() =
    
    [<Test>]
    member this.TestSprintf() =
        let s = sprintf "%s is %d years old" "Mary" 5
        s |> should equal "Mary is 5 years old"

    [<Test>]
    member this.TestMultipleLines() =
        let s = "a\
        b\
        cd"
        s |> should equal "abcd"

    [<Test>]
    member this.TestCompare()=
        (compare "hello" "cheka") |> should equal 5
        (compare "Hello" "hello") |> should equal -32

    [<Test>]
    member this.TestArrayFeature() = 
        let s = "cheka"
        s.Length |> should equal 5
        s.[0] |> should equal 'c'
        s.[4] |> should equal 'a'

        let substring : string = s.[1..3]
        substring |> should equal "hek"

    /// in F#, string is mapped into "System.String" class, so API provided by "System.String" are available
    [<Test>]
    member this.TestSystemStringFeature() = 
        let s = "hello" + string 2011
        s |> should equal "hello2011"
        s.Length |> should equal 9
        (s.EndsWith("11")) |> should be True

    [<Test>]
    member this.TestStringBuilder()=
        let sb = System.Text.StringBuilder()
        sb.Append("hello ") |> ignore
        sb.Append("wsu").Append(" from F#") |> ignore
        sb.ToString() |> should equal "hello wsu from F#"

    [<Test>]
    member this.TestToNumber()=
        let str = "101"
        (int str) |> should equal 101
        (double str) |> should (equalWithin 1e-6) 101.0

        (fun()-> (int "f#") |> ignore ) |> should throw typeof<System.FormatException>
