
namespace TestFSharps

type FsEventSource(name : string) =
    let mutable m_name = name
    let m_numChangeEvt = new Event<int>()

    member this.Name 
        with get() = m_name
        and set newvalue = m_name <- newvalue

    member this.Num
        with set newvalue = m_numChangeEvt.Trigger newvalue

    [<CLIEvent>]
    member this.NumChangeEvent = m_numChangeEvt.Publish
