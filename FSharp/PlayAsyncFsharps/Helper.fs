
namespace PlayAsyncFsharps

open System
open System.Threading

module Helper=
    let tprintfn format=
        printf "[Thread-%d] " Thread.CurrentThread.ManagedThreadId
        printfn format

    let pause()=
        printfn "Press ENTER to continue, ......"
        Console.ReadLine() |> ignore

    let console2agent (agent : MailboxProcessor<string>) = 
        let rec loop() = 
            printf ">>> "
            let input = Console.ReadLine()
            match input with
                | "exit" -> ()
                | _ ->
                    agent.Post input 
                    loop()
        loop() 
