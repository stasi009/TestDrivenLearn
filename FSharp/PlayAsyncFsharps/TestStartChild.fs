
namespace PlayAsyncFsharps

open Helper

module TestStartChild =
    let private sync_delay (name: string) (period : int)=
        async {
            tprintfn "'%s' starts, ......" name
            System.Threading.Thread.Sleep period
            tprintfn "'%s' ends" name
            return period
        }

    let private async_delay (name:string) (period : int)=
        async {
            tprintfn "'%s' starts, ......" name
            do! Async.Sleep period
            tprintfn "'%s' ends" name
            return period
        }

    /// always occupy the same thread (in this case, the main thread, with id=1)
    let private test_without_child delay =
        let parent = async {
            let! result1 = delay "child1" 2000
            let! result2 = delay "child2" 500
            return (sprintf "results = <%d,%d>" result1 result2)
        }
        printfn "%s" (Async.RunSynchronously parent)

    /// chekanote: queue multiple child-async-job into thread-pool
    let private test_startchild delay =
        let parent = async {
            // run multiple children in parallel, even the async-job is actually blocking ones
            let! asyncjob1 = delay "child1" 2000 |> Async.StartChild
            let! asyncjob2 = delay "child2" 500 |> Async.StartChild
            tprintfn "all subtasks started." 

            let! result1 = asyncjob1
            let! result2 = asyncjob2
            return (sprintf "results = <%d,%d>" result1 result2)
        }
        printfn "%s" (Async.RunSynchronously parent)
 
    let main() =
        // test_without_child sync_delay
        test_without_child async_delay

        pause()

        test_startchild sync_delay

