
namespace PlayAsyncFsharps.Agent

open PlayAsyncFsharps.Helper

module AgentError =

    let private demo() = 
        let receiver = MailboxProcessor.Start(fun inbox->
            let rec loop counter = async {
                try
                    let! msg = inbox.Receive 1000
                    let number = int msg
                    tprintfn "%d-th number received: %d" counter number
                with 
                    | :? System.TimeoutException -> tprintfn "!!! timeout exception"
                return! loop (counter + 1)
            }
            loop 1
        )

        // chekanote: after the error is thrown, whether you attach callback or not
        // the 'MailboxProcessor' won't be available any more
        // cannot be useful any more
        receiver.Error.Add (fun ex ->
            match ex with
                | :? System.FormatException -> tprintfn "input cannot be parsed as string"
                | _ -> 
                    tprintfn "unknown exception: %s" ex.Message
                    raise ex
                    // cannot use 'reraise' outside "try...with..."
        )

        console2agent receiver

 
    let main()=
        demo()

