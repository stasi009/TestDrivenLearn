
namespace ChatRoom

open System
open System.Text
open System.Net
open System.Net.Sockets

open PlayAsyncFsharps.Helper
open PlayAsyncFsharps.TestFromBeginEndUdp

type RequestInfo = {
    Agent: UdpAgent;
    Buffer: byte[];
    Size: int;
    SrcEndpnt : IPEndPoint;
}
and UdpAgent (m_processor : RequestInfo->Async<unit>) as this =
    // ************************ member fields
    let m_socket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp)

    let m_mailbox = new MailboxProcessor<UdpMessage>(fun inbox->
        let rec loop() = async{
            let! msg = inbox.Receive()

            try
                match msg with
                    | Received(buffer,size,srcpoint) -> 
                        let info = {Agent = this;Buffer = buffer;Size=size;SrcEndpnt=srcpoint}
                        do! m_processor info
                    | ToSend(buffer,destendpoint) ->
                        let!_ = m_socket.FsAsyncSendTo buffer 0 buffer.Length destendpoint
                        ()
            with
                | Failure(msg) -> printfn "!!! Error Caught: %s" msg

            return! loop()
        }
        loop()
    )

    // ************************ user-defined constructor
    new(processor : RequestInfo->unit) = new UdpAgent(fun info -> async {processor info})

    // ************************ public API
    member this.Start localPort =
        m_mailbox.Start()
                
        m_socket.Bind(new IPEndPoint(IPAddress.Any,localPort))
        printfn "server is listening on port<%d> now, ......" localPort

        let rec loop() = async{
            let! (bytes,recvSize,srcEndpnt) = m_socket.FsAsyncReceiveFrom 1024
            m_mailbox.Post (Received(bytes,recvSize,srcEndpnt))
            return! loop()
        }
        loop() |> Async.StartImmediate

    member this.Send buffer destEndPoint = 
        m_mailbox.Post (ToSend(buffer,destEndPoint))

    interface IDisposable with 
        member this.Dispose() =
            m_socket.Close()
            (m_mailbox :> IDisposable).Dispose()

module TestUdpAgent =
    let private LsnPort = 7027

    let private run_server() =
        let counter = ref 0
        
        use server = new UdpAgent (fun info->
            incr counter

            let msg = Encoding.ASCII.GetString(info.Buffer,0,info.Size)
            printfn "[%d] '%s' received from '%s'" !counter msg (info.SrcEndpnt.ToString())

            let retbytes = Encoding.ASCII.GetBytes(sprintf "echo - %s" msg)
            info.Agent.Send retbytes info.SrcEndpnt
        )
        server.Start LsnPort

        pause()

    let private run_client()=
        let sendingInterval = 20
        let processInterval = 100
        let svrEndpoint = new IPEndPoint(IPAddress.Loopback,LsnPort)

        use client = new UdpAgent(fun info ->
            let msg = Encoding.ASCII.GetString(info.Buffer,0,info.Size)
            printfn "%s" msg
            System.Threading.Thread.Sleep processInterval
        )
        client.Start (LsnPort + 1)

        seq {
            for index = 1 to 1000 do
                System.Threading.Thread.Sleep sendingInterval
                yield index
        } |> Seq.iter (fun item->
                            let bytes = Encoding.ASCII.GetBytes (string item)
                            client.Send bytes svrEndpoint 
                            )

        pause()

    let main()=
        let startOption = Environment.GetCommandLineArgs().[1]
        match startOption with
            | "server" -> run_server()
            | "client" -> run_client()
            | _ -> failwith "unrecognized startup option"

