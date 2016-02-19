

#load "webserver.fsx"
#r "Newtonsoft.Json.dll"

open System
open System.Net
open System.Web
open System.Text
open System.Collections.Specialized
open Newtonsoft.Json

// class 'WebServerBase' has default namespace 'Webserver'
// that namespace is the same as the file name, but with the first letter upper-case
open Webserver 
open HttpListenerExtend

type Product = {
    Id : int
    Name : string
    Price : float
}

type Server4Get(url,m_total) =
    inherit WebServerBase(url)

    let m_products = Array.init m_total (fun index -> {Id=index;Name=sprintf "product%d" index;Price=100.0*(float index)})

    override this.HandleRequest context = async {
        match context.Request.Url.LocalPath with
        | "/get" -> 
            let namevalues = context.Request.QueryString
            let startindex = int <| namevalues.Get("from")
            let endindex = int <| namevalues.Get("to")

            if startindex >= 0 && startindex <= endindex && endindex < m_total then
                let reply = JsonConvert.SerializeObject m_products.[startindex..endindex]
                do! context.Response.AsyncReply(reply,"application/json")
                printfn "records from %d to %d are sent back" startindex endindex
            else
                context.Response.StatusCode <- int HttpStatusCode.Forbidden
                do! context.Response.AsyncReply("index over range")
                printfn "!!! ERROR: index[%d..%d] over range" startindex endindex

            return true
        | _ ->
            return false
    }

do
    use server = new Server4Get("http://localhost:50000/",10)
    server.Start()
    printfn "server is started, ......"
    pause()
