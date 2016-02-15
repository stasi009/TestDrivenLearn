
namespace PlayAsyncFsharps

open Helper

/// two people died today (Oct 5, 2011)
/// 1. Steve Jobs (1955 ~ 2011), died at 56. Pay my tribute to IT genius
/// 2. Ruoxi died at 34. Thank her to let me still have a sweet dream about love. 
module TestFromContinuation =

    let wrapper f argument = 
        let from_body ((cont : 't->unit),econt,ccont) =
            try
                let result = f argument
                cont result
            with 
                | :? System.OperationCanceledException as oce -> ccont oce
                | ex -> econt ex

        Async.FromContinuations from_body

    let test_wrapper() =
        // -------------- successful result
        let result = (wrapper (fun num->num * num ) 2) |> Async.RunSynchronously 
        printfn "success result=%d" result

        // -------------- exception
        try
            wrapper (fun()-> failwith "test exception") () |> Async.RunSynchronously
            failwith "impossible to run this line"
        with
            |Failure(msg) -> printfn "!!!!!! exception caught: '%s'" msg

        // -------------- cancel
        let cts = new System.Threading.CancellationTokenSource()

        try
            let loop = fun()->
                            let rec _loop counter : unit=
                                cts.Token.ThrowIfCancellationRequested()
                                printfn "loop-%d completed" counter
                                System.Threading.Thread.Sleep 100
                                _loop (counter + 1)
                            _loop 1

            async {
                do! Async.Sleep 1000
                cts.Cancel()
            } |> Async.StartImmediate

            wrapper loop () |> Async.RunSynchronously
        with
            | :? System.OperationCanceledException as oce->printfn "!!!!!! cancel exception caught"

    let my_asleep (interval : float) =
        // the cont will be assigned to following instructions in the async{}
        let job (cont : unit->unit,_,_) = 
            let timer = new System.Timers.Timer(interval)
            timer.AutoReset <- false

            let onTimeout evtargs = 
                timer.Stop()
                timer.Dispose()
                cont()
            timer.Elapsed.Add onTimeout

            timer.Start()

        Async.FromContinuations job

    let test_my_asleep()=
        async {
            let stopwatch = System.Diagnostics.Stopwatch.StartNew()
            tprintfn "------------ started"

            do! my_asleep 1500.0
            tprintfn "------------ finished, totally cost [%d]ms" stopwatch.ElapsedMilliseconds
        } |> Async.StartImmediate
        
        pause()            

    let main() =
        // test_wrapper()
        test_my_asleep()

