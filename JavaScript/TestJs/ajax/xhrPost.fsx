
#load "simpleWebServer.fsx"
open SimpleWebServer
open HttpListenerExtend

open System
open System.Net

type Server(url) =
    inherit SimpleWebServerBase(url)

    let mutable m_counter = 0

    override this.HandleRequest context = async {
        match context.Request.Url.LocalPath with
        | "/testpost" -> 
            m_counter <- m_counter + 1

            let! content = context.Request.AsyncInputString
            printfn "[%d] received: %s" m_counter content

            do! context.Response.AsyncReply "ok"
            return true
        | _ -> 
            return false
    }

do
    use server = new Server("http://localhost:50000/")
    server.Start()
    printfn "server is started, ......"
    pause()

