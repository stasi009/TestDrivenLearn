
namespace PlayAsyncFsharps

open System.Threading

open Helper

module TestCancel=
    let block_loops (total:int) (interval:int) =
        async{
            printfn "############### SYNC-loop starts, ......"
            // loop within async{} will check CancelToken automatically and implicitly
            // we don't need to check it explicitly and manually
            for index = 1 to total do
                Thread.Sleep interval
                printfn "loop-%d completed" index
            printfn "############### SYNC-loop ends"
        }

    let nonblock_loops (total : int) (interval:int)=
        async{
            tprintfn "############### ASYNC-loop starts, ......"
            for index = 1 to total do
                do! Async.Sleep interval
                tprintfn "loop-%d completed" index
            tprintfn "############### ASYNC-loop ends"
        }

    let test_canceldefault()=
        let check (loops: Async<unit>)=
            loops |> Async.Start

            pause()
            Async.CancelDefaultToken()

            pause()

        // check (block_loops 100 100) // loop in async{} will check cancellation
        check (nonblock_loops 100 100) // let! or do! within async{} check cancellation

    let test_custom_canceltoken()=
        let check (loops : Async<unit>) =
            let cts = new CancellationTokenSource() 

            Async.Start(loops,cts.Token)

            pause()
            cts.Cancel()

            pause()

        // check (block_loops 100 100)
        check (nonblock_loops 100 100)

    let recurseloop_cannot_cancel_bydefault()=
        let rec loops counter = 
            Thread.Sleep 100
            printfn "loop-%d finishes" counter
            if counter = 100 then ()
            else loops (counter + 1)

        async {loops 1} |> Async.Start

        pause()
        Async.CancelDefaultToken()
        printfn "!!!!!!!!!!!!!!!!!!!!!!! 'CancelDefaultToken' is invoked."// cannot stop recursive loop

        pause()

    let cancel_recurseloop_manualcheck()=
        let rec loop_until_cancel (counter : int) (ct: CancellationToken)=
            Thread.Sleep 100
            printfn "loop-%d finishes" counter
            if (ct.IsCancellationRequested) then ()
            else loop_until_cancel (counter + 1) ct

        let asyncjob = async {
            let! ct = Async.CancellationToken
            loop_until_cancel 1 ct
            printfn "!!!!!!!!!!!! loops completed."
        }

        Async.Start asyncjob

        pause()
        Async.CancelDefaultToken()

        pause()

    let check_trycancel (total: int) (cancellimit: int)=
        async {
            do! Async.Sleep cancellimit
            Async.CancelDefaultToken()
            printfn "!!!!! default token cancelled."
        } |> Async.StartImmediate
        printfn "cancel job is running, ......"

        // we don't use Async.Sleep here, for demonstrating that 
        // async{} will check cancellation during loop
        let job = async {
            for index = 1 to total do
                Thread.Sleep 100
                printfn "loop-%d completed." index
            
            printfn "successfully returned."
            return 1000
        }

        let cancellable = Async.TryCancelled(job,fun oce->printfn "!!!!! cancel callback: job is cancelled.")

        try
            let result = cancellable |> Async.RunSynchronously
            printfn "successfully get result=%d" result
        with 
            | :? System.OperationCanceledException  -> printfn "!!!!! OperationCanceledException is caught"

    let test_trycancel() = 
        printfn "############################ Cancelled"
        check_trycancel 1000 1000 // cancelled
        pause()

        printfn "############################ Run until end"
        check_trycancel 10 1000000 // run successfully until end

    let test_oncancel() =
        let loop (id) = async {
            tprintfn "operation[%d] starts" id
            // the function passed into "OnCancel" will always be executed
            // on the thread that is performing the cancellation
            use! disposable = Async.OnCancel(fun() -> tprintfn "operation[%d] is disposed" id  )
            while true do
                do! Async.Sleep 100
            tprintfn "!!!!!!!!!! cancel trigger exception, this line should never be executed"
        }

        let start id =
            let cts = new CancellationTokenSource()
            Async.Start(loop(id),cts.Token)
            id,cts

        [|1;2|] 
        |> Array.map start
        |> Array.iter (fun (id,cts) ->
                                pause()
                                tprintfn "cancel signal is sent to operation[%d]" id
                                cts.Cancel())

        pause()


    let main()=
        // test_canceldefault()
        // test_trycancel()
        test_oncancel()
        // test_custom_canceltoken()
        // cancel_recurseloop_manualcheck()
        // recurseloop_cannot_cancel_bydefault()
        

