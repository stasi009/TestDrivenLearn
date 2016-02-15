
namespace PlayAsyncFsharps.Agent

open System

type CountMessage = 
    | Stop
    | Increment of int
    | Fetch of AsyncReplyChannel<int> 

[<Sealed>]
type CounterAgent()=
    let m_mailbox = MailboxProcessor.Start(fun inbox->
        let rec loop sum = async{
            let! msg = inbox.Receive()
            match msg with
                | Stop ->
                    printfn "agent stopped !!!" 
                    return ()
                | Increment(value) -> return! loop (sum + value)
                | Fetch(channel) ->
                    channel.Reply sum
                    // !!! we cannot use do! here
                    // !!! although using do! will not cause stack-overflow
                    // !!! however, do! will leak memory
                    // !!! so we must use return! to launch tail-recursive call
                    return! loop sum
        }
        loop 0
    )

    member this.Stop()=
        m_mailbox.Post Stop

    member this.Increment value=
        m_mailbox.Post (Increment(value))

    member this.Fetch(?timeout : int)=
        m_mailbox.PostAndReply((fun channel -> Fetch(channel)),?timeout=timeout)

    member this.AsyncFetch(?timeout)= 
        m_mailbox.PostAndAsyncReply((fun channel->Fetch(channel)),?timeout=timeout)

    interface IDisposable with
        member this.Dispose() = (m_mailbox :> IDisposable).Dispose()

