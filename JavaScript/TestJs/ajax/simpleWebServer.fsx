
#r "FSharp.PowerPack.dll"

open System
open System.IO
open System.Net
open System.Text

module HttpListenerExtend =
    type HttpListener with
        member this.AsyncGetContext() =
            Async.FromBeginEnd(this.BeginGetContext,this.EndGetContext)

    type HttpListenerRequest with        
        member this.AsyncInputString = async {
            use reader = new StreamReader(this.InputStream,this.ContentEncoding)
            return! reader.AsyncReadToEnd()
        }

    type HttpListenerResponse with
        member this.AsyncReply( contType,buffer: byte[]) = async {
            try
                this.ContentLength64 <- int64 buffer.Length
                this.ContentType <- contType
                do! this.OutputStream.AsyncWrite buffer
            finally
                this.OutputStream.Close()
        }

        member this.AsyncReply (text : string) = async {
            try
                let buffer = Encoding.UTF8.GetBytes text
                this.ContentLength64 <- int64 buffer.Length
                this.ContentType <- "text/html; charset=UTF-8"
                do! this.OutputStream.AsyncWrite buffer
            finally
                this.OutputStream.Close()
        }

open HttpListenerExtend

[<AbstractClass>]
type SimpleWebServerBase(url)=
    // ******************** primary constructor
    let m_listener = new HttpListener()
    let m_contentTypes = 
        [".css","text/css";
        ".html","text/html";
        ".htm","text/html";
        ".js","text/javascript"]
        |> Map.ofSeq

    do
        m_listener.Prefixes.Add url

    // ******************** public API
    abstract HandleRequest: HttpListenerContext -> Async<bool>

    abstract PreDispose : unit -> unit
    default this.PreDispose() = ()

    member this.Start() =
        m_listener.Start()

        let rec loop() = async {
            let! result = m_listener.AsyncGetContext() |> Async.Catch
            match result with
            | Choice1Of2 context ->
                let! answer = this.HandleRequest context |> Async.Catch
                match answer with
                | Choice1Of2 handled ->
                    if not handled then
                        // then it must be "file request"
                        let fileName = 
                            let localpath = context.Request.Url.LocalPath
                            if localpath.Equals("/") then
                                "index.htm"
                            else
                                localpath.TrimStart('/').TrimStart('\\')

                        if (File.Exists fileName) then
                            let ext = Path.GetExtension(fileName).ToLower()
                            let contType = m_contentTypes.[ext]
                            let contents = File.ReadAllBytes fileName
                            do! context.Response.AsyncReply(contType,contents)
                        else
                            let response = context.Response
                            response.StatusCode <- int HttpStatusCode.NotFound
                            response.StatusDescription <- "Not Found"

                            let errMsg = sprintf "File Not Found: %s" fileName
                            do! response.AsyncReply errMsg

                            if not (fileName.EndsWith "ico") then
                                // because this error message is so frequent
                                // and doesn't matter much, so we choose to ignore 
                                // "cannot find ico" warning
                                printfn "%s" errMsg

                    return! loop()
                | Choice2Of2 ex ->
                    printfn "!!! Error[%s]: %s" (ex.GetType().Name) ex.Message

            | Choice2Of2 ex ->
                printfn "!!! Error[%s]: %s" (ex.GetType().Name) ex.Message
        }

        loop() |> Async.StartImmediate

    interface IDisposable with
        member this.Dispose() =
            this.PreDispose()
            m_listener.Stop()
            m_listener.Close()

let pause()=
    printfn "Press ENTER to continue, ......"
    Console.ReadLine() |> ignore

        
    


