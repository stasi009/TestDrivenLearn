
namespace PlayAsyncFsharps.Agent

open System

open PlayAsyncFsharps.Helper

module AgentReceive =
    let private test_iterative_loop()=
        use agent = MailboxProcessor.Start(fun inbox-> async {
            try
                // we can use mutable variables here, because there is only one reader
                // multiple writers, one reader
                let counter = ref 0
                while true do
                    let! msg = inbox.Receive()

                    if msg.Equals("stop") then
                        failwith "use exception to exit"// because F# doesn't provide "break"
                    else
                        incr counter
                        tprintfn "%d-th: '%s' received" !counter msg 
            with
                | Failure(msg)-> printfn "!!!!!!!!!!!! %s" msg
        })

        console2agent agent

    let private test_recursive_loop()=
        use agent = MailboxProcessor.Start(fun inbox ->
            let rec loop counter = async {
                let! msg = inbox.Receive()
                if msg.Equals("stop") then 
                    tprintfn "loop over !!!"
                    return ()
                else
                    tprintfn "%d-th: '%s' received" counter msg
                    do! loop (counter + 1)
            }
            loop 1
        )
        console2agent agent

    let private test_receive_timeout()=
        use agent : MailboxProcessor<string> = MailboxProcessor.Start(fun inbox->
            let rec loop counter = async {
                try
                    let! msg = inbox.Receive 1000
                    if msg.Equals("stop") then
                        tprintfn "loop over !!!"
                        return ()
                    else 
                        tprintfn "%d-th: '%s' received" counter msg
                        do! loop (counter + 1)
                with
                    | :? System.TimeoutException -> 
                        tprintfn "!!!!!!!!!! receive timeout"
                        do! loop (counter + 1)
            }
            loop 1
        )
        console2agent agent

    let private test_tryreceive()=
        use agent = MailboxProcessor.Start(fun inbox->
            let rec loop counter = async {
                let! opt = inbox.TryReceive 1000
                match opt with
                    | None->
                        tprintfn "############# receive timeout"
                        do! loop (counter + 1)
                    | Some(msg)->
                        if msg.Equals("stop") then
                            tprintfn "loop over !!!"
                            return ()
                        else 
                            tprintfn "%d-th: '%s' received" counter msg
                            do! loop (counter + 1)
            }
            loop 1
        )
        console2agent agent
        
    let main()=
        // test_iterative_loop()
        test_recursive_loop()
        // test_receive_timeout()
        // test_tryreceive()

