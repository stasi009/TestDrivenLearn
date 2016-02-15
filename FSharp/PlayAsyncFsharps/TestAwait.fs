
namespace PlayAsyncFsharps

open Helper

module TestAwait =

    (*
    Async.AwaitEvent, from MSDN 
    "Creates an asynchronous computation that waits for a single invocation of a CLI event by adding a handler to the event. Once the computation completes or is cancelled, the handler is removed from the event."
    so it is safe to be used in a loop, the event handler is detached everytime
    as a result, it won't cause any resource leak
    *)
    let test_await_events()=
        let rec evtwait_loop (evt : IEvent<string>) = async {
            let! msg = Async.AwaitEvent evt

            if msg.Equals("exit") then
                printfn "########## finish waiting the events"
                return ()
            else
                printfn "msg<%s> received." msg
                return! evtwait_loop evt
        }

        let rec input_loop (evt : Event<string>) =
            let input = System.Console.ReadLine()
            
            if input.Equals("done") then ()
            else 
                evt.Trigger input
                input_loop evt
                
        let evtsource = new Event<string>()
        evtwait_loop evtsource.Publish |> Async.StartImmediate
        printfn "begin waiting the events."

        printfn "begin waiting inputs, ......"
        input_loop evtsource

    let test_await_waithandler()=
        let event = new System.Threading.ManualResetEventSlim(false)

        async {
            do! Async.Sleep 2000
            event.Set()
        } |> Async.StartImmediate

        async {
            tprintfn "begin waiting on the WaitHandler, ......"
            let! flag = Async.AwaitWaitHandle event.WaitHandle
            tprintfn "waiting completes with result=%b" flag
        } |> Async.RunSynchronously

    let test_await_task()=
        let task = System.Threading.Tasks.Task.Factory.StartNew(
                        new System.Func<int>(fun()-> 
                                        System.Threading.Thread.Sleep 2000
                                        printfn "task completed."
                                        99)
        )
        let result = (async {
            tprintfn "begin waiting the task, ......"
            return! Async.AwaitTask task 
        } |> Async.RunSynchronously)
        printfn "result = %d" result
 
    let main()=
        // test_await_events()
        // test_await_waithandler()
        test_await_task()

