
namespace TestFSharps

open NUnit.Framework
open FsUnit

module Module4Exception=
    // exception defined by keyword "exception" are F# exception, other than .NET exception
    exception MyFsException of string * int
    
    type Fool()=
        let mutable m_finallyInvoked = false

        member this.FinallyInvoke with get() = m_finallyInvoked

        member this.Run (hasError: bool)=
            try
                if hasError then
                    failwith "error"
                else
                    "ok"
            finally
                m_finallyInvoked <- true // finally expression won't determine the return type

        member this.ThrowFsException errmsg errId=
            raise (MyFsException(errmsg,errId))

        member this.ThrowDotNetException errmsg =
            raise (System.InvalidOperationException(errmsg))

    let safe_divide (x:int) (y:int)=
        try
            Some(x/y)
        with
            | :? System.DivideByZeroException -> None

    let precaution_divide (x:int) (y:int)=
        if y = 0 then
            invalidArg "y" "divided by zero"
        else
            x/y
open Module4Exception

[<TestFixture>]
type TestExceptions()=
    class
        [<Test>]
        member this.TestCatch()=
            let result1 = safe_divide 5 2
            result1.Value |> should equal 2

            let result2 = safe_divide 4 0
            result2.IsNone |> should be True

        [<Test>]
        member this.TestFinally()=
            let okfool = new Fool()
            okfool.FinallyInvoke |> should be False
            (okfool.Run false) |> should equal "ok"
            okfool.FinallyInvoke |> should be True 

            let errorfool = new Fool()
            errorfool.FinallyInvoke |> should be False
            (fun() -> (errorfool.Run true) |> ignore) |> should throw typeof<System.Exception>
            errorfool.FinallyInvoke |> should be True

        // for F# exception, there is no need to use ":?" when matching the pattern
        [<Test>]
        member this.TestCustomFsException()=
            let result = 
                try
                    let f = new Fool()
                    f.ThrowFsException "f# exception" 123
                    ("",-1)
                with
                    | MyFsException(msg,id) -> (msg,id)
            result |> should equal ("f# exception",123)

        [<Test>]
        member this.TestDotNetException()=
            let result =
                try
                    let f = new Fool()
                    f.ThrowDotNetException "dotnet exception"
                    "impossible"
                with
                    | :? System.InvalidOperationException as error -> error.Message
            result |> should equal "dotnet exception"

        [<Test>]
        member this.TestReraise()=
            let result =
                try
                    try
                        (new Fool()).ThrowFsException "test" 11
                    with
                        | MyFsException(msg,id)-> reraise()
                with
                    | MyFsException(msg,id)->(msg,id)
            result |> should equal ("test",11)

        [<Test>]
        member this.TestFailwith()=
            let fool hasError errmsg =
                // both routine, whether successful or throw exception,
                // they must both return the same type of results
                try
                    if hasError then failwith errmsg
                    else "ok"
                with
                    | Failure(msg) -> msg

            (fool false "") |> should equal "ok"
            (fool true "error") |> should equal "error"

        [<Test>]
        member this.TestFailwithCannotFormat() =
            let fool no =
                try
                    // although it isn't syntax error
                    // but it won't make effect, the string will not be formated
                    failwith "error#%d" no
                with
                    |Failure(msg) -> msg

            (fool 6) |> should equal "error#%d" 

        [<Test>]
        member this.TestInvalidArg()=
            let fool x y =
                try
                    let result = precaution_divide x y
                    string result
                with
                    | :? System.ArgumentException as ex-> ex.Message

            (fool 10 3) |> should equal "3"
            (fool 10 0) |> should equal "divided by zero\r\nParameter name: y"

        [<Test>]
        member this.TestCatchGeneralException() =
            let fool ()=
                raise <| System.InvalidOperationException("invalid")

            let msg = 
                try
                    fool()
                    "ok"
                with
                    | ex -> ex.Message

            msg |> should equal "invalid"

    end