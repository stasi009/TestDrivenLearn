
namespace PlayAsyncFsharps

open System.Threading

open Helper

module TestExceptions=
    let private test_trycatch()=
        let subroutine = async {
            do! Async.SwitchToThreadPool()
            
            tprintfn "begin job,......"
            failwith "test exception"
            tprintfn "impossible to run this code"

            return 100
        }

        let caller = async {
            try
                let! result = subroutine
                failwith "impossible to run this code"
            with
                | Failure(msg) -> tprintfn "exception caught: '%s'" msg
        }

        tprintfn "main starts"
        caller |> Async.StartImmediate

        pause()

    let private test_finally() =
        let job = async {
            try
                do! Async.Sleep 1000
                failwith "error for test"
            finally
                printfn "!!!!!! finally is invoked. !!!!!!"
        }

        try
            Async.RunSynchronously job
        with
            | Failure(msg) ->
                printfn "error caught: %s" msg

    let checkexception_inparallel (successjob: Async<int>) =
        let failurejob = async {
            do! Async.Sleep 500
            tprintfn "!!!!!!!!!!!!! job failed."
            failwith "test exception"
            return -1
        }

        let results = [successjob;failurejob] |> Async.Parallel |> Async.Catch |> Async.RunSynchronously
        match results with
            | Choice1Of2 _ -> failwith "impossible to get results"
            | Choice2Of2 ex -> tprintfn "!!!!!!!!!!!!! exception caught: '%s'" ex.Message

    /// chekanote:  failed job can cancel other async-jobs
    let testparallelexception_asyncloop()=
        let successjob = 
            let rec loop counter = async {
                if counter = 1000 then
                    tprintfn "successfully returning, ......"
                    return counter
                else
                    do! Async.Sleep 100
                    tprintfn "%d-th loop succeeds" counter
                    return! loop (counter + 1)
            }
            loop 1

        checkexception_inparallel successjob

    /// chekanote:  if the other jobs are recursive, and those recursive loop are synchronous
    /// then the failed job CANNOT cancel those running ones to stop
    /// the whole job "Parallel |> RunSynchronously" can only stop when the long-successful-running ones
    /// are totally completed.
    /// OR precisely, cannot be cancelled by default token. to cancel it, we can pass in user-defined token
    /// and explicitly cancel it in the failure job
    let testparallelexception_sync_recurse() =
        let successjob = async {
            let rec loop counter = 
                if counter = 100 then
                    tprintfn "successfully returning, ......"
                    counter
                else
                    Thread.Sleep 100
                    tprintfn "%d-th loop succeeds" counter
                    loop (counter + 1)
            return loop 1
        }
        checkexception_inparallel successjob

    /// chekanote: the other running jobs are synchronous, but they are not recursive
    /// but uses loop, those running ones can be cancelled
    let testparallelexception_syncloop() = 
        let successjob = async {
            for index = 1 to 1000 do
                Thread.Sleep 10
                tprintfn "%d-th loop succeeds" index

            tprintfn "successfully returning, ......"
            return 1000
        }
        checkexception_inparallel successjob

    let main()=
        // test_trycatch()
        // testparallelexception_asyncloop()
        // testparallelexception_syncloop()
        // testparallelexception_sync_recurse()
        test_finally()
        