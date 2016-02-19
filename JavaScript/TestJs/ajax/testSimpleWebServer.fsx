
#load "simpleWebServer.fsx"
open SimpleWebServer

open System
open System.Net

type FileServer(url) =
    inherit SimpleWebServerBase(url)

    // never handle any request, just forward to "file transferring"
    override this.HandleRequest context = async {
        return false
    }

do
    use server = new FileServer("http://localhost:50000/")
    server.Start()
    printfn "server is started, ......"
    pause()

