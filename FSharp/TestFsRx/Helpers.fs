namespace TestFsRx

open System
open System.Reactive.Linq

module Helpers = 
    let pause() = 
        printfn "Press <Enter> to continue, ......"
        Console.ReadLine() |> ignore
    
    let consoleRead2Observable() = 
        let onSubscribed (observer : IObserver<string>) = 
            let disposeAct = new Action(fun () -> printfn "observer unsubscribed")
            
            let rec loop() = 
                printf ">>> "
                let line = Console.ReadLine()
                match line.ToLower() with
                | "exit" -> 
                    observer.OnCompleted()
                    disposeAct
                | "error" -> 
                    observer.OnError(new ArgumentException("user input 'error'"))
                    disposeAct
                | _ -> 
                    observer.OnNext line
                    loop()
            loop()
        Observable.Create onSubscribed
