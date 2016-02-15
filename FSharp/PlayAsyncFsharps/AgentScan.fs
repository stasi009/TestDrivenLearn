
namespace PlayAsyncFsharps.Agent

open PlayAsyncFsharps.Helper

module AgentScan =
    type private Messages =
        | Msg1 
        | Msg2 of int
        | Msg3 of string
    
    let private test_scan() = 
        let agent = MailboxProcessor.Start(fun inbox->
            let rec loop() : Async<unit> =
                inbox.Scan(function
                        | Msg1 -> None
                        | Msg2(num) -> 
                            Some(async{
                                printfn "msg2 received with <%d>" num
                                do! loop()
                            })
                        | Msg3(str) ->
                            Some(async{
                                printfn "msg3 received with <%s>" str
                                do! loop()
                            })
                )// scan
            loop()
        )

        [Msg1;Msg3("stasi");Msg2(100);Msg3("hello f#");Msg1;Msg2(88)]
            |> List.iter (fun item->agent.Post item)

        pause()
        printfn "current length of the queue = %d" agent.CurrentQueueLength

    let private test_filter()=
        let agent = MailboxProcessor.Start(fun inbox->
            let rec loop counter = async{
                do! inbox.Scan(fun num->
                    if num%2 =0 then 
                        Some(async{
                            printfn "%d-th even number found" counter
                            do! loop (counter + 1)
                        })
                    else
                        None
                )// scan
            }// loop definition
            loop 1
        )

        seq {
            let rand = new System.Random()
            for index = 1 to 10 do
                System.Threading.Thread.Sleep 100
                let num = rand.Next()
                printfn "--------------- [%d] generated" num
                yield num
        } |> Seq.iter (fun num->agent.Post num)

        pause()
        printfn "current length of the queue = %d" agent.CurrentQueueLength

    /// use Scan to receive the messages in a specific order
    let private test_specific_recv_order()=
        let mbox = MailboxProcessor.Start(fun inbox->
            let filter_msg2 = function
                    | Msg2(num) -> Some(async.Return num)
                    | _ -> None

            let filter_msg3 = function
                    | Msg3(content) -> Some(async.Return content)
                    | _ -> None

            // receive messages in order: first msg2, only after then, msg3
            async {
                let! number = inbox.Scan filter_msg2
                printfn "msg2 received with number=%d" number

                let! content = inbox.Scan filter_msg3
                printfn "msg3 received with content='%s'" content
            }
        )

        // first post msg3, but it will not be dequeued and processed
        // because it is still waiting for msg2 
        mbox.Post (Msg3 "hello stasi")
        printfn "*************** Msg3 has been posted"
        pause()

        // at this time, first msg2 is dequeued and processed
        // and then, long-waiting msg3 is poped out and processed 
        mbox.Post (Msg2 99)
        printfn "*************** Msg2 has been posted"
        pause()

 
    let main() = 
        // test_scan()
        // test_filter()
        test_specific_recv_order()

