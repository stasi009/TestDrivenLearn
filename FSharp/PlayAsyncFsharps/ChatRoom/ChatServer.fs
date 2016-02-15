
namespace ChatRoom

open System
open System.Text

type ChatServer()=
    // ######################## member fields
    let m_splitter = [|' '|]

    let m_chatagent = new ChatAgent()

    let m_udpagent = new UdpAgent(fun info-> async {
        let msg = Encoding.ASCII.GetString(info.Buffer,0,info.Size)
        let segments = msg.Split( m_splitter,2,StringSplitOptions.RemoveEmptyEntries)

        match segments.[0] with
            | Constants.CmdSend ->
                m_chatagent.Send segments.[1]
            | Constants.CmdClear ->
                m_chatagent.Clear()
            | Constants.CmdGet ->
                let! contents = m_chatagent.AsyncGet()
                let retbuffer = Encoding.ASCII.GetBytes contents
                info.Agent.Send retbuffer info.SrcEndpnt
            | _ -> failwith "unrecognized command"
    }
    )

    // ######################## public API
    member this.Start localPort =
        m_udpagent.Start localPort

    interface IDisposable with 
        member this.Dispose() =
            (m_udpagent :> IDisposable).Dispose()
            (m_chatagent :> IDisposable).Dispose()
