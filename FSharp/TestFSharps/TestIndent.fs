
namespace TestFSharps

open NUnit.Framework
open FsUnit

// "class" can write in the same line 
[<TestFixture>]
type TestIdent() = class

    [<Test>]
    member this.Sample1()=
        // the first character of "let", which is "l", introduce the offside
        // this multiple line construct, the following lines must be indented further
        let value =
            let a = 1
            let b = 2
            a + b
        value |> should equal 3

    [<Test>]
    member this.Sample2()=
        let add a b = a + b

        // chekanote: we can write the arguments in multiple lines, the argument in another line
        // must be indented further then the first character of the function name
        let sum1 = add 100
                    200
        sum1 |> should equal 300

    [<Test>]
    member this.Sample3()=
        // chekanote: "undent" is allowed for lambda expression, but at least, I don't like it
        ([1..3] |> List.map (fun item->
                let temp = item * item
                string temp)) |> should equal ["1";"4";"9"]

    [<Test>]
    member this.Sample4()=
        let sequence = seq {
            yield 1
            yield 2
            
            let a = 5
            yield a * a
        }
        sequence |> should equal [1;2;25]

        (* below code will generate compiling error
        let sequence2 = seq 
        {
            yield "cheka"
            yield "wsu"
        }
        *)


end