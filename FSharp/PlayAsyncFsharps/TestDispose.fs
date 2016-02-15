
namespace PlayAsyncFsharps

open System
open System.Threading

open Helper

module TestDispose =
    let make_dispose_flag()=
        {
            new IDisposable with
                member this.Dispose()= printfn "!!!!! disposed !!!!!"
        }

    let test_dispose_when_success()=
        let fool= async {
            use disposable = make_dispose_flag()

            printfn "start working, ......"
            Thread.Sleep 200
            printfn "complete working."
        }

        fool |> Async.Start

        pause()

    let test_dispose_when_exception()=
        let fool = async {
            use disposable = make_dispose_flag()

            printfn "start working, ......"
            failwith "test exception"
            printfn "impossible to run this line"
        }

        let result = fool |> Async.Catch |> Async.RunSynchronously
        match result with
            | Choice1Of2 _ -> failwith "impossible"
            | Choice2Of2 ex -> printfn "caught exception: '%s'" ex.Message

    let main() =
        // test_dispose_when_success()
        test_dispose_when_exception()

