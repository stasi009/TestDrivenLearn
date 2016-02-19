

#load "webserver.fsx"

open System
open System.Net
open System.Web
open System.Text
open System.Collections.Specialized

// class 'WebServerBase' has default namespace 'Webserver'
// that namespace is the same as the file name, but with the first letter upper-case
open Webserver 
open HttpListenerExtend

type Server4Load(url) =
    inherit WebServerBase(url)

    let form_html httpmethod (kvs : NameValueCollection) =
        let sb = new StringBuilder(sprintf "<p>%s returns: </p><ol>" httpmethod)

        for index = 0 to kvs.Count-1 do
            let line = sprintf "<li>Name=%s, Value=%s</li>" (kvs.GetKey(index)) (kvs.Get(index))
            sb.Append line |> ignore
        sb.Append("</ol>") |> ignore

        sb.ToString()

    let handle_get (context : HttpListenerContext) = async {
        if context.Request.HttpMethod.Equals("get",StringComparison.OrdinalIgnoreCase) then
            let reply = form_html "GET" context.Request.QueryString
            do! context.Response.AsyncReply reply
        else
            raise <| new InvalidOperationException("only accept GET method")
    }

    let handle_post (context : HttpListenerContext) = async {
        if context.Request.HttpMethod.Equals("post",StringComparison.OrdinalIgnoreCase) then
            let! body = context.Request.AsyncInputString
            let kvs = HttpUtility.ParseQueryString body
            let reply = form_html "POST" kvs
            do! context.Response.AsyncReply reply
        else
            raise <| new InvalidOperationException("only accept POST method")
    }

    override this.HandleRequest context = async {
        match context.Request.Url.LocalPath with
        | "/loadbyget" -> 
            do! handle_get context
            return true
        | "/loadbypost"->
            do! handle_post context
            return true
        | _ ->
            return false
    }

do
    use server = new Server4Load("http://localhost:50000/")
    server.Start()
    printfn "server is started, ......"
    pause()
