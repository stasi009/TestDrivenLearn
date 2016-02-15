namespace TestFsRx

open System
open System.Threading
open System.Reactive.Linq
open System.Reactive.Concurrency

module TestCreate = 
    let private sample1() = 
        let asequence = { 1..10 }
        let interval = TimeSpan.FromSeconds 1.0
        
        let onSubscribed (observer : IObserver<int>) = 
            for e in asequence do
                observer.OnNext e
                Thread.Sleep interval
            observer.OnCompleted()
            new Action(fun () -> printfn "subscription disposed")
        
        let stream = Observable.Create<int>(onSubscribed)
        stream.Subscribe(printfn "event: %d") |> ignore
    
    // Create will generate a cold-observable
    // then SubscribeOn a new thread, means the cold-observable will start on a new thread
    // if there is on ObserveOn, then all callbacks will be executed on that new thread
    let private sample_subscribeon (scheduler : IScheduler) (tag : string) = 
        let oristream = Helpers.consoleRead2Observable()
        
        let srcstream = 
            if scheduler = null then oristream
            else oristream.SubscribeOn(scheduler)
        srcstream.Subscribe((fun txt -> printfn "\t%s\n" txt), (fun () -> printfn "!!! data source closed !!!")) 
        |> ignore
        printfn "%s" tag
        (new ManualResetEvent(false)).WaitOne()
    
    let main (argv : string []) = 
        // sample1()
        // sample_subscribeon null "synchronously subscribe on blocking data source"
        // Scheduler.Default is thread-pool in Console Applications
        // sample_subscribeon Scheduler.Default "asynchronously subscribe on blocking data source"
        sample_subscribeon NewThreadScheduler.Default "asynchronously subscribe on blocking data source"
