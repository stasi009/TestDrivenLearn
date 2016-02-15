
namespace PlayAsyncFsharps

open System
open System.Threading
open System.Threading.Tasks

open Helper

module TestStart=
    /// although it has been wrapped in the asyn{}, but it is still a blocking function
    let private sync_sleep (id: int) (sleeptime : int)=
        async {
            tprintfn "<%d> begin sleeping, ......" id
            Thread.Sleep sleeptime
            tprintfn "<%d> wake up" id
        }

    let private async_sleep (id:int) (sleeptime: int)=
        async {
            tprintfn "<%d> begin sleeping, ......" id
            do! Async.Sleep sleeptime
            tprintfn "<%d> wake up" id
        }

    // *************** TESTER FUNCTIONS *************** //
    let private test_sync_sleep()=
        [(1,1500);(2,1000)]
            |> List.map (fun (id,sleeptime)-> sync_sleep id sleeptime)
            |> List.iter (fun job->Async.StartImmediate job)
        printfn "######### blocked until all async-job done"

    let private test_async_sleep()=
        [(100,2500);(200,2000)]
            |> List.map (fun (id,sleeptime) -> async_sleep id sleeptime)
            |> List.iter (fun job-> job |> Async.StartImmediate)

        printfn "all async-job starts"
        pause()


    let private demo_start()=
        let sleep_print = async {
            Thread.Sleep 1000
            tprintfn "\tsync-sleep done."

            do! Async.Sleep 1000
            tprintfn "\tasync-sleep done."
        }
        let check caption f = 
            tprintfn "!!!!!!! before %s" caption
            f()
            tprintfn "!!!!!!! after %s" caption
            pause()
            printfn "\n"

        check "Start" (fun() -> sleep_print |> Async.Start)
        check "StartImmediate" (fun() -> sleep_print |> Async.StartImmediate)
        check "RunSynchronously" (fun() -> sleep_print |> Async.RunSynchronously)
        // 'StartWithContinuations' also starts on current thread
        check "StartWithContinuations" (fun()->
                                            Async.StartWithContinuations(
                                                sleep_print,
                                                (fun _ -> tprintfn "completes"),
                                                raise,
                                                ignore))
        check "StartAsTask" (fun() ->
                                let task = Async.StartAsTask sleep_print
                                task.ContinueWith( Action<Task>(fun _->tprintfn "task completes") )|> ignore)

    let private test_delayfeature()=
        let environment = ref 0
        printfn "before define the async-job, environment=%d" !environment

        // this is because async builder automatically call "async.Delay"
        // for each async expression
        let job = async {
            do! Async.Sleep 500
            tprintfn "############ in async-workflow, environment=%d" !environment
        }

        environment := 888
        printfn "environment changed to %d after defining the async-job" !environment

        // the correct result should print changed value by the async workflow
        Async.RunSynchronously job

        environment := 666
        printfn "environment changed again to %d" !environment
        Async.RunSynchronously job

    // chekanote: this test demonstrate that Async<T> is not a value
    // but represents a piece of codes that will be executed in the feature
    // it will be executed repeatedly from start to end each time being called
    // the same feature like Seq
    let private test_reexecute_feature() =
        
        let counter = ref 100

        let job = async {
            do! Async.Sleep 500
            counter := !counter + 1
            return !counter
        }

        for index = 1 to 3 do
            let result = Async.RunSynchronously job
            printfn "%d" result

    let main() =
        // test_sync_sleep()
        // test_async_sleep()
        // demo_start()
        // test_delayfeature()
        test_reexecute_feature()
