namespace PlayAsyncFsharps

open System
open System.Text
open System.Net
open System.Net.Sockets

module TestFromBeginEndUdp = 
    // ****************************** extensions
    type Socket with
        
        member this.FsAsyncSendTo (bytes : byte array) (offset : int) (length : int) (destEndpnt : EndPoint) : Async<int> = 
            let _beginsendto ((_buffer, _offset, _size, _destEndpnt : EndPoint), callback, state) = 
                this.BeginSendTo(_buffer, _offset, _size, SocketFlags.None, _destEndpnt, callback, state)
            Async.FromBeginEnd((bytes, offset, length, destEndpnt), _beginsendto, this.EndSendTo)
        
        member this.FsAsyncReceiveFrom(maxSize : int) : Async<byte [] * int * IPEndPoint> = 
            let buffer = Array.zeroCreate<byte> maxSize
            let endpoints = [| new IPEndPoint(IPAddress.Any, 0) :> EndPoint |]
            let _beginReceiveFrom (callback, state) = 
                this.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, &(endpoints.[0]), callback, state)
            
            let _endReceiveFrom asyncResult = 
                let received = this.EndReceiveFrom(asyncResult, &(endpoints.[0]))
                (buffer, received, endpoints.[0] :?> IPEndPoint)
            Async.FromBeginEnd(_beginReceiveFrom, _endReceiveFrom)
    
    // ****************************** classes
    type Server() = 
        let m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
        
        let rec loop (socket : Socket) (counter : int) = 
            async { 
                printfn "[%d] waiting, ......" counter
                let! (bytes, recvSize, srcEndpnt) = socket.FsAsyncReceiveFrom 1024
                let msg = Encoding.ASCII.GetString(bytes, 0, recvSize)
                printfn "[%d] '%s' received from '%s'" counter msg (srcEndpnt.ToString())
                let retbytes = Encoding.ASCII.GetBytes(sprintf "echo - %s" msg)
                let! _ = socket.FsAsyncSendTo retbytes 0 retbytes.Length srcEndpnt
                printfn "[%d] echo back\n" counter
                return! loop socket (counter + 1)
            }
        
        member this.Start(localPort : int) = 
            m_socket.Bind(new IPEndPoint(IPAddress.Any, localPort))
            loop m_socket 1 |> Async.StartImmediate
        
        interface IDisposable with
            member this.Dispose() = m_socket.Close()
    
    type SyncClient() = 
        let m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
        
        member this.Run (svrEndpnt : EndPoint) (msg : string) = 
            async { 
                let sendbytes = Encoding.ASCII.GetBytes msg
                let! _ = m_socket.FsAsyncSendTo sendbytes 0 sendbytes.Length svrEndpnt
                let! (recvbuffer, size, _) = m_socket.FsAsyncReceiveFrom 1024
                return Encoding.ASCII.GetString(recvbuffer, 0, size)
            }
        
        interface IDisposable with
            member this.Dispose() = m_socket.Close()
    
    type AsyncClient(m_svrEndpnt : IPEndPoint, m_callback : string -> unit) = 
        let m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
        
        let m_recvQ = 
            MailboxProcessor.Start(fun inbox -> 
                let rec loop() = 
                    async { 
                        let! msg = inbox.Receive()
                        m_callback msg
                        return! loop()
                    }
                loop())
        
        member this.Send(msgs : string seq) = 
            msgs |> Seq.iter (fun msg -> 
                        let bytes = Encoding.ASCII.GetBytes msg
                        m_socket.FsAsyncSendTo bytes 0 bytes.Length m_svrEndpnt
                        |> Async.Ignore
                        |> Async.StartImmediate)
        
        member this.BeginReceive(localPort : int) = 
            let rec loop() = 
                async { 
                    let! (recvbuffer, size, _) = m_socket.FsAsyncReceiveFrom 1024
                    let recvmsg = Encoding.ASCII.GetString(recvbuffer, 0, size)
                    m_recvQ.Post recvmsg
                    return! loop()
                }
            m_socket.Bind(new IPEndPoint(IPAddress.Any, localPort))
            loop() |> Async.StartImmediate
        
        interface IDisposable with
            member this.Dispose() = 
                m_socket.Close()
                (m_recvQ :> IDisposable).Dispose()
    
    // ****************************** functions
    let SvrPort = 7027
    
    let run_server() = 
        use server = new Server()
        server.Start SvrPort
        Helper.pause()
    
    let run_sync_client() = 
        let svrEndpnt = new IPEndPoint(IPAddress.Loopback, SvrPort)
        use client = new SyncClient()
        
        let rec loop() = 
            let input = Console.ReadLine()
            match input with
            | "exit" -> ()
            | _ -> 
                let recvmsg = (client.Run svrEndpnt input |> Async.RunSynchronously)
                printfn "'%s' received from server\n" recvmsg
                loop() // tail recursion
        loop()
    
    let run_async_client() = 
        let sendingInterval = 20
        let processInterval = 100
        
        use client = 
            new AsyncClient(new IPEndPoint(IPAddress.Loopback, SvrPort), 
                            fun msg -> 
                                printfn "%s" msg
                                System.Threading.Thread.Sleep processInterval)
        client.BeginReceive(SvrPort + 1)
        seq { 
            for index = 1 to 1000 do
                System.Threading.Thread.Sleep sendingInterval
                yield index
        }
        |> Seq.map (fun item -> string item)
        |> client.Send
        Helper.pause()
    
    let main() = 
        let startOption = Environment.GetCommandLineArgs().[1]
        match startOption with
        | "server" -> run_server()
        | "sync-client" -> run_sync_client()
        | "async-client" -> run_async_client()
        | _ -> failwith "unrecognized startup option"
