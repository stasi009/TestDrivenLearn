namespace TestFSharps

open NUnit.Framework
open FsUnit

module Module4Events = 
    type FunInt2Str = delegate of int -> string
    
    // for delegate of multiple arguments, those arguments must be wrapped into a tuple
    type MultiArgsFunc = delegate of (int * string) -> string
    
    /// IEvent derives from IObservable, so it should comply with the Observable Contract
    /// including all callbacks are invoked in sequence (guarantation for serial notification)
    let private test_sequence() = 
        let src = new Event<string>()
        src.Publish.Add(fun arg -> 
            printfn "\tbegin callback, ......'"
            System.Threading.Thread.Sleep 1000
            printfn "\tend callback with args '%s'" arg)
        let trigger arg = 
            printfn "begin trigger, ......"
            src.Trigger arg
            printfn "end trigger\n"
        [ "hello"; "f#"; "wsu" ] |> List.iter trigger
        printfn "############# Finished #############"
    
    let test_main() = test_sequence()

type EvtSource(m_id : int) = 
    let m_event = new Event<_>() // let the compiler to inference the type
    
    // making this event compatible with .NET 
    [<CLIEvent>]
    member this.Event = m_event.Publish
    
    member this.Id = m_id
    member this.FireMsg(msg : string) = 
        // we don't need to first check whether the Event is null or not
        m_event.Trigger(this, msg) // fire the arguments as a tuple

type EvtSink() = 
    let mutable m_srcId : int = -1
    let mutable m_msg : string = null
    member this.Received = (m_srcId, m_msg)
    // receive the arguments as a tuple
    member this.Handler(sender : EvtSource, msg : string) = 
        m_srcId <- sender.Id
        m_msg <- msg

[<TestFixture>]
type TestEvents() = 
    
    [<Test>]
    member this.TestDelegate() = 
        let delegate1 = new Module4Events.FunInt2Str(fun n -> string n)
        (delegate1.Invoke 8) |> should equal "8"
        let delegate2 = new Module4Events.FunInt2Str(fun n -> sprintf "hello%d" n)
        (delegate2.Invoke 9) |> should equal "hello9"
        let delegate3 = new Module4Events.MultiArgsFunc(fun (num, name) -> sprintf "%d-%s" num name)
        (// call such delegate, parameters must also be wrapped into a tuple
         delegate3.Invoke(100, "xy")) |> should equal "100-xy"
    
    [<Test>]
    member this.TestClassicalEvents() = 
        let src = new EvtSource(100)
        src.FireMsg "wasted" // fire before subscription won't have any problem
        let sink = new EvtSink()
        src.Event.Add sink.Handler
        src.FireMsg "hello f#"
        sink.Received |> should equal (100, "hello f#")
        src.FireMsg "stasi"
        sink.Received |> should equal (100, "stasi")
    
    [<Test>]
    member this.TestNonclassicalEvents() = 
        let sb = new System.Text.StringBuilder()
        let src = new Event<string>()
        src.Publish.Add(fun msg -> sb.Append(msg) |> ignore)
        src.Trigger("hello ")
        src.Trigger("f#")
        sb.ToString() |> should equal "hello f#"
    
    [<Test>]
    member this.TestScan() = 
        let sb = new System.Text.StringBuilder()
        let evt = new Event<string>()
        // "scanned" event triggers everytime its state is re-calculated
        evt.Publish
        |> Event.scan (fun state _ -> state + 1) 0
        |> Event.add (fun arg -> sb.Append(string arg) |> ignore)
        for index = 1 to 3 do
            evt.Trigger "hello"
        sb.ToString() |> should equal "123"
    
    [<Test>]
    member this.TestFunctionalFeature() = 
        let received = new System.Collections.Generic.List<int * string>()
        let src = new EvtSource(99)
        src.Event
        |> Event.filter (fun (sender, msg) -> msg.Length >= 3)
        |> Event.add (fun (sender, msg) -> received.Add((sender.Id, msg)))
        [ "hello"; "a"; "xyz"; "bc"; "cheka" ] |> List.iter src.FireMsg
        received |> should equal [ (99, "hello")
                                   (99, "xyz")
                                   (99, "cheka") ]
