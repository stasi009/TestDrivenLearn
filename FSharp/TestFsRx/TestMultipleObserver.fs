namespace TestFsRx

open System
open System.Reactive.Linq
open System.Reactive.Concurrency

module TestMultipleObserver = 
    (*
    this test shows when the source stream is ObserveOn(ThreadPool) or ObserveOn(Default)
    it will have the effect of parallel load sharing, that is to say, multiple subscribers will run in parallel
    you can see that multiple observers will start simultaneously
    however, if you didn't specify where to ObserveOn, then multiple observers will run in sequence
    you will see that the second observer will only start after the first observer is finished
    so the conclusion is: when a single source stream is observed on ThreadPool or TaskPool, then multiple observers will run in parallel
    *)
    let private demo1 (scheduler : IScheduler) = 
        let srcProxyStream = Observable.Interval(TimeSpan.FromSeconds(5.0)).Publish()
        
        let make_subscriber name interval = 
            fun (i : int64) -> 
                async { 
                    printfn "--- '%s' starts" name
                    do! Async.Sleep interval
                    printfn "+++ '%s': %d" name i
                }
                |> Async.RunSynchronously
        
        let forkStream = 
            if scheduler = null then srcProxyStream :> IObservable<int64>
            else srcProxyStream.ObserveOn(scheduler)
        
        [| ("s1", 1000)
           ("s2", 2000) |]
        |> Seq.iter (fun (name, interval) -> 
               make_subscriber name interval
               |> forkStream.Subscribe
               |> ignore)
        srcProxyStream.Connect() |> ignore
        Helpers.pause()
    
    let main (argv : string []) = 
        // demo1 ThreadPoolScheduler.Instance
        demo1 Scheduler.Default
// demo1 null
