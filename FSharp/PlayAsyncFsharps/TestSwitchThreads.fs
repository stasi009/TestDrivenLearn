
namespace PlayAsyncFsharps

open System
open System.Threading

open Helper

module TestSwitchThreads=
    let private child_async_func (num : int)=
        async {
            let threadid = Thread.CurrentThread.ManagedThreadId
            Console.WriteLine("\tbegin child at thread<{0}>,......",threadid)
            Thread.Sleep 2000
            Console.WriteLine("\tend child at thread<{0}>,......",threadid)

            return num * num
        }

    let private parent_async_func (num : int) =
        async {
            tprintfn "begin parent,......"

            let! result = child_async_func num

            tprintfn "parent resumed with result=%d" result
        }

    /// for CPU-bound async job, even with "Async.Start"
    /// it won't schedule its child async-job into thread pool, but use the same parent-pooled-thread
    /// to execute the child async-job, parent and child async-job share the same thread
    let private test_schedule_threadpool()=
        tprintfn "main function starts"
        (parent_async_func 10) |> Async.Start

        pause()

    let private test_schedule_currentthread()=
        tprintfn "main function starts"
        parent_async_func 10 |> Async.StartImmediate
        tprintfn "main function ends"

    /// from this function, we can see that Async.Parallel uses "Thread Pool"
    /// each async-job runs in parallel
    let private test_parallel()=
        [1..3] 
            |> List.map child_async_func
            |> Async.Parallel
            |> Async.Ignore
            |> Async.StartImmediate

        pause()// since Async.Parallel doesn't block, so we must manually pause the whole program

    let private switch_demo() = 
        let asyncjob = async{
            tprintfn "current thread"
            
            do! Async.SwitchToNewThread()
            tprintfn "new thread"

            do! Async.SwitchToThreadPool()
            tprintfn "thread pool"
        }
        Async.StartImmediate asyncjob

        pause()

    let main() =
        // test_schedule_threadpool()
        // test_schedule_currentthread()
        // test_parallel()
        switch_demo()