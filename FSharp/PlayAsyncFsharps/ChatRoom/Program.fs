// Learn more about F# at http://fsharp.net

open System
open System.Net

open PlayAsyncFsharps.Helper
open ChatRoom


module TestChatRoom =
    let private LsnPort = 7027

    let private run_server()=
        use server = new ChatServer()
        server.Start LsnPort

        pause()

    let private run_client()=
        use client = new ChatClient( new IPEndPoint(IPAddress.Loopback,LsnPort),LsnPort+1 )

        let rec loop() =
            printfn ">>> "
            let input = Console.ReadLine()
            match input with
                | "exit" -> 
                    printfn "done!!!"
                    ()
                | _ -> 
                    client.Request input
                    loop()
        loop()

    let main()=
        let startOption = Environment.GetCommandLineArgs().[1]
        match startOption with
            | "server" -> run_server()
            | "client" -> run_client()
            | _ -> failwith "unrecognized startup option"

// TestChatAgent.main()
// TestUdpAgent.main()
TestChatRoom.main()