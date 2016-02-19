
#load "simpleWebServer.fsx"
open SimpleWebServer
open HttpListenerExtend

open System
open System.Net

type Server(url,m_template) =
    inherit SimpleWebServerBase(url)

    let mutable m_counter = 0

    override this.HandleRequest context = async {
        match context.Request.Url.LocalPath with
        | "/testget" -> 
            m_counter <- m_counter + 1

            let msgback = sprintf "[%d] %s" m_counter m_template
            do! context.Response.AsyncReply msgback

            printfn "%d requests served" m_counter
            return true
        | "/clearall"->
            m_counter <- 0

            do! context.Response.AsyncReply "yes" // request approved

            printfn "##################### ALL CLEARED #####################"
            return true
        | _ -> 
            return false
    }

do
    use server = new Server("http://localhost:50000/","stasi's ajax")
    server.Start()
    printfn "server is started, ......"
    pause()

