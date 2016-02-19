

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
    mutable Id : int
    Name : string
    Price : float
}

type Server4Post(url) =
    inherit WebServerBase(url)
    
    let m_products = new ResizeArray<Product>()
    let mutable m_seed = 10000

    let handle_post (context : HttpListenerContext) = async {
        let! json = context.Request.AsyncInputString
        
        let newproducts = JsonConvert.DeserializeObject<Product[]> json
        newproducts
        |> Array.iter (fun p ->
                            m_seed <- m_seed + 1
                            p.Id <- m_seed)

        m_products.AddRange newproducts
        printfn "%d new products are added" newproducts.Length

        context.Response.EmptyReply()
    }

    let handle_get (context : HttpListenerContext) = async {
        let json = JsonConvert.SerializeObject m_products
        do! context.Response.AsyncReply(json,"application/json")
        printfn "%d records are sent back" m_products.Count
    }

    override this.HandleRequest context = async {
        match context.Request.Url.LocalPath with
        | "/post" -> 
            do! handle_post context
            return true
        | "/get"->
            do! handle_get context
            return true
        | _ ->
            return false
    }

do
    use server = new Server4Post("http://localhost:50000/")
    server.Start()
    printfn "server is started, ......"
    pause()
