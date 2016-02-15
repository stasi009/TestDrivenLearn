
namespace ChatRoom

open System

[<Sealed>]
type internal ChatAgent()=
    let m_mailbox = MailboxProcessor.Start(fun inbox->
        let rec loop received = async{
            let! msg = inbox.Receive()
            match msg with
                | Send(content) -> 
                    return! loop (content :: received)
                | Clear ->
                    return! loop []
                | Get(channel) ->
                    let sb = new System.Text.StringBuilder()
                    received |> List.iter (fun item->(sb.Append (sprintf "%s;" item)) |> ignore)
                    channel.Reply (sb.ToString())
                    return! loop received
        }
        loop []
    )

    member this.Send msg = 
        m_mailbox.Post (Send(msg))

    member this.Clear() =
        m_mailbox.Post (Clear)

    member this.Get() = 
        m_mailbox.PostAndReply(fun channel->Get(channel))

    member this.AsyncGet()=
        m_mailbox.PostAndAsyncReply(fun channel->Get(channel))

    interface IDisposable with 
        member this.Dispose() = (m_mailbox :> IDisposable).Dispose()

module TestChatAgent=
    let main() =
        use agent = new ChatAgent()

        agent.Send "hello"
        agent.Send "wsu"
        printfn "get: '%s'" (agent.Get())

        agent.Clear()
        agent.Send "stasi"
        agent.Send "kgb"
        printfn "get: '%s'" (agent.Get())
