
namespace ChatRoom

open System
open System.Text
open System.Net

type ChatClient(m_svrEndpnt : IPEndPoint,localPort:int)=
    // ------------------------ member fields
    let m_splitter = [|';'|]

    let m_agent = new UdpAgent(fun info->
        let msg = Encoding.ASCII.GetString(info.Buffer,0,info.Size)
        let segments = msg.Split(m_splitter,StringSplitOptions.RemoveEmptyEntries) 
        for index = 0 to segments.Length-1 do
            printfn "\t[%d] %s" (segments.Length - index) segments.[index]
    )
    do m_agent.Start localPort

    // ------------------------ public API
    member this.Request (command : string) =
        let bytes = Encoding.ASCII.GetBytes command
        m_agent.Send bytes m_svrEndpnt

    member this.Send msg =
        if not (String.IsNullOrEmpty msg) then
            this.Request (sprintf "%s %s" Constants.CmdSend msg)

    member this.Clear()=
        this.Request Constants.CmdClear

    member this.Get()=
        this.Request Constants.CmdGet

    interface IDisposable with 
        member this.Dispose()=
            (m_agent :> IDisposable).Dispose()

