
namespace PlayAsyncFsharps

open System
open System.Text
open System.Net
open System.Net.Sockets

open Helper

module TestFromBeginEndTcp =
    // ******************** extensions ******************** //
    type Socket with
        member this.FsAsyncAccept() =
            Async.FromBeginEnd(this.BeginAccept,this.EndAccept)

        member this.FsAsyncConnect (svrEndpoint : EndPoint)=
            Async.FromBeginEnd(
                svrEndpoint,
                (fun (endpnt,callback,state) -> this.BeginConnect(endpnt,callback,state)),
                this.EndConnect
            )

    // ******************** classes ******************** //
    type Server() =
        let m_lsnSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp)

        member this.Start (lsnPort:int) (prefix : string)=
            let rec loop (lsnSocket : Socket) counter = async{
                // chekanote: the purpose using do! here is to make a small scope
                // which allows the "use" and "use!" inside can be automatically be invoked
                // otherwise, because of the recursive call, it never exit the external
                // scope and those Dispose will never be invoked
                do! async {
                    printfn "\nAccepting %d-th connection, ......" counter
                    use! clientSocket = lsnSocket.FsAsyncAccept()
                    use clientStream = new NetworkStream(clientSocket,true)
                    printfn "%d-th client accepted" counter

                    let buffer = Array.zeroCreate<byte> 1024
                    let! received = clientStream.AsyncRead(buffer,0,buffer.Length)
                    let msg = Encoding.ASCII.GetString(buffer,0,received)
                    printfn "*** message [%s] received" msg

                    let returnbytes = Encoding.ASCII.GetBytes (sprintf "%s - %s" prefix msg) 
                    let! _ = clientStream.AsyncWrite returnbytes

                    use disposeFlag = {
                        new IDisposable with 
                            member this.Dispose()=
                                printfn "%d-th client completed." counter
                    }
                    ()
                }
                return! loop lsnSocket (counter + 1)
            }

            m_lsnSocket.Bind(new IPEndPoint(IPAddress.Loopback,lsnPort))
            m_lsnSocket.Listen 10

            (loop m_lsnSocket 1) |> Async.StartImmediate

        interface IDisposable with
            member this.Dispose()=
                m_lsnSocket.Close()

    type Client() =
        let m_socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp)
        let mutable m_disposed = false

        let _run (svrEndpoint: IPEndPoint) (msg :string) = async{
            use disposeFlag = {
                new IDisposable with
                    member this.Dispose() =
                        m_disposed <- true
                        printfn "client closed."
            }

            do! m_socket.FsAsyncConnect svrEndpoint
            use stream = new NetworkStream(m_socket,true)
            printfn "server connected."

            let bytes = Encoding.ASCII.GetBytes msg
            let! _ = stream.AsyncWrite bytes
            printfn "<%s> sent to server" msg

            let buffer = Array.zeroCreate<byte> 1024
            let! received = stream.AsyncRead(buffer,0,buffer.Length)
            return (Encoding.ASCII.GetString(buffer,0,received))
        }

        member this.Run (svrEndpoint : IPEndPoint) (msg:string)= 
            _run svrEndpoint msg |> Async.RunSynchronously

        interface IDisposable with 
            member this.Dispose() =
                if not m_disposed then
                    m_socket.Shutdown(SocketShutdown.Both)
                    m_socket.Close()

    // ******************** constants ******************** //
    let LsnPort = 7027

    // ******************** methods ******************** //
    let run_server()=
        use server = new Server()
        server.Start LsnPort "echo"

        pause()

    let run_client()=
        let rec loop input = 
            if input.Equals "exit" then 
                ()
            else
                use client = new Client()
                let svrEndpoint = new IPEndPoint(IPAddress.Loopback,LsnPort)

                let recvmsg = client.Run svrEndpoint input
                printfn "############ <%s> received from server\n" recvmsg

                loop (Console.ReadLine())// tail recursion
        loop (Console.ReadLine())
    
    let main()=
        let arguments = Environment.GetCommandLineArgs()
        if arguments.[1].Equals("server") then
            run_server()
        elif arguments.[1].Equals("client") then
            run_client()
        else
            failwith (sprintf "un-recognized startup option: %s" arguments.[1])

