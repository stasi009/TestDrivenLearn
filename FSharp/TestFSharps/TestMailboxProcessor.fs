
namespace TestFSharps.Async

open System.Threading
open Microsoft.FSharp.Control

module internal Module4Mailbox=
    type Message(m_id:int,m_content:string)=
        static let mutable m_idbase = 0

        member this.Id with get() = m_id
        member this.Content with get() = m_content

        static member CreateMessage content=
            m_idbase <- m_idbase + 1
            new Message(m_idbase,content)

    type CounterMsg =
        | Stop
        | Increment of int
        | FetchCurrent of AsyncReplyChannel<int>

    type CounterAgent() =
        class

        // --------------- private member field
        let m_mailbox = MailboxProcessor.Start(fun inbox->
            let rec loop currentNum = async {
                let! received = inbox.Receive()
                match received with
                    | Stop ->
                        printfn "CounterAgent STOPPED" 
                        return ()
                    | Increment(adding) -> do! loop(currentNum + adding)
                    | FetchCurrent(reply) ->
                        reply.Reply(currentNum)
                        do! loop currentNum
            }
            loop 0)

        // --------------- public API
        member this.Increment adding =
            m_mailbox.Post(Increment(adding))

        member this.FetchCurrent() =
            m_mailbox.PostAndReply(fun reply->FetchCurrent(reply))

        member this.Stop() =
            m_mailbox.Post Stop

        end

    let test_post()=
        let agent =  new MailboxProcessor<Message>(fun inbox->
            let rec loop counter = async{
                printfn "\twaiting for %d-th message on thread<%d>, ......" counter Thread.CurrentThread.ManagedThreadId
                let! msg = inbox.Receive()
                printfn "\tmessage received. id=%d,content='%s'" msg.Id msg.Content
                do! loop (counter + 1)
            }
            loop 1)

        agent.Start()

        let rec loop() =
            printf ">>> "
            let input = System.Console.ReadLine()

            if input.Equals("stop") then ()
            else 
                agent.Post (Message.CreateMessage input)
                loop()
        loop()

    let test_sync_reply()=
        let agent = new CounterAgent()

        let fetch_print()=
            let result = agent.FetchCurrent()
            printfn "current summation = %d" result
            result

        agent.Increment 50
        agent.Increment 38
        assert (fetch_print() = 88)

        agent.Increment 12
        assert (fetch_print() = 100)

        agent.Stop()
        Module4AsyncWorkflow.pause()


    let test_main()=
        // test_post()
        test_sync_reply()
