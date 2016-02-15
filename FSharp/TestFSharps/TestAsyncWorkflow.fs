
namespace TestFSharps.Async

open System
open System.Threading;

module Module4AsyncWorkflow=
    let pause() =
        printfn "Press any key to continue, ......"
        System.Console.ReadLine() |> ignore

    let blocking_delay (name:string) (sleeptime: float)=
        async {
            let threadid = (Thread.CurrentThread.ManagedThreadId)
            printfn "%s is sleeping in thread<%d>, ......" name threadid
            Thread.Sleep (TimeSpan.FromSeconds sleeptime)
            printfn "%s wake up." name

            return (name, sleeptime,threadid)
        }

    let simple_sample()=
        [("<1>",1.0);("\t<2>",2.5);("\t\t<3>",4.0)]
            |> List.map (fun item->blocking_delay (fst item) (snd item))
            |> Async.Parallel
            |> Async.RunSynchronously
            |> Seq.iter (fun item ->
                            match item with
                                | (name,sleeptime,threadid)->
                                    printfn "!!! %s has slept %3.2f seconds on thread<%d>" name sleeptime threadid)

    let test_letbang_blocking()=
        let fool name sleeptime=
            async{
                let! result = blocking_delay name sleeptime
                match result with
                    | (rn,rtime,rthid)->
                        printfn "currentthread=%d,<%s> wake up from thread<%d>" 
                            (Thread.CurrentThread.ManagedThreadId) rn rthid
            }
        (fool "w" 3.0) |> Async.StartImmediate
        printfn "############## StartImmediate completed. ##############"

    /// RunSynchronously catch the exception internally, can re-throw the exception on the caller thread
    let test_exception_runSync()=
        let fool total= async{
            for index = 1 to total do
                Thread.Sleep 100
                printfn "loop-%d finished" index
            failwith "test"
        }

        try
            (fool 10) |> Async.RunSynchronously
        with 
            | Failure(msg)->printfn "!!!!! error caught, message='%s'" msg

    let test_async_catch()=
        let fool delaytime hasError= async{
            Thread.Sleep (TimeSpan.FromSeconds delaytime)
            
            if hasError then 
                failwith "test"
            
            return delaytime
        }

        let catch_fool delaytime hasError=
            let result = (fool delaytime hasError) |> Async.Catch  |> Async.RunSynchronously
            match result with
                | Choice1Of2 answer -> printfn "answer is %f" answer
                | Choice2Of2 ex -> printfn "!!! error message=%s" ex.Message

        catch_fool 2.0 true
        catch_fool 3.0 false

    let test_sleep_notInterrupt()=
        let sleepjob = async {
            let starttm = System.DateTime.Now
            printfn "begin sleeping, ......"

            do! Async.Sleep 5000

            let endtm = System.DateTime.Now;
            let elapsed = endtm - starttm
            printfn "end sleeping after %3.2f seconds" elapsed.TotalSeconds
        }
        let onCanceledCallback (ex: OperationCanceledException )=
            printfn "!!!!!!!! %s" ex.Message

        Async.TryCancelled( sleepjob,onCanceledCallback)|> Async.Start

        let canceljob = async {
            do! Async.Sleep 1000
            Async.CancelDefaultToken()
            printfn "default token is canceled"
        }
        canceljob |> Async.Start
        pause()

    let cancelable_blockloop (total:int) (canceltoken: CancellationToken) =
        async{
        for index = 1 to total do
            canceltoken.ThrowIfCancellationRequested()
            Thread.Sleep 100
            printfn "loop-%d finished" index
        }

    let cancelable_nonblock_loop (total:int) (canceltoken: CancellationToken) =
        async{
        for index = 1 to total do
            canceltoken.ThrowIfCancellationRequested()
            do! Async.Sleep 100
            printfn "loop-%d finished" index
        }

    let test_cancel()=
        let cts = new CancellationTokenSource()
        
        let cancelablejob = Async.TryCancelled((cancelable_blockloop 100 cts.Token),(fun ex->printfn "!!!!! %s" ex.Message)) 

        Async.Start(cancelablejob,cts.Token)

        pause()
        cts.Cancel()

        pause()

    let test_startwithcontinuation()=
        let cts = new CancellationTokenSource()

        //!!! chekanote:  cannot use any block here, because 'StartWithContinuations' starts immediately on the current thread
        Async.StartWithContinuations(
                        (cancelable_nonblock_loop 20 cts.Token),
                        (fun() -> printfn "###### completed"),
                        (fun ex ->printfn "!!!!!! error<%s> caught" ex.Message),
                        (fun _ -> printfn "****** cancelled"),cts.Token)

        pause()
        cts.Cancel()

        pause()

    let test_main()=
        // simple_sample()
        test_letbang_blocking()
        // test_exception_runSync()
        // test_async_catch()
        // test_sleep_notInterrupt()
        // test_cancel()
        // test_startwithcontinuation()
