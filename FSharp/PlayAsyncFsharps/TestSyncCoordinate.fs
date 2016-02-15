
namespace PlayAsyncFsharps

open Helper

module TestSyncCoordinate =

    type SafeCounter()=
        let mutable m_counter = 0
        let m_lock = new obj()

        member this.Value = m_counter

        /// we don't need to use Reference Cell here to modify the member field
        member this.Increase()=
            lock m_lock (fun()-> m_counter <- m_counter + 1;m_counter)

    let private test_lock()=
        let counter = new SafeCounter()

        let _factory index (loopCount,sleeptime)=
            let rec loop remaining = async {
                if remaining = 0 then
                    tprintfn "!!! worker[%d] completes" index
                    return ()
                else 
                    do! Async.Sleep sleeptime
                    let newvalue = counter.Increase()
                    tprintfn "worker[%d] increase counter to %d" index newvalue
                    return! loop (remaining - 1)
            }
            loop loopCount

        let inputs = [(3,500);(4,300);(5,100)] 
        let expected = inputs |> List.fold (fun acc (num,_) -> acc + num) 0

        inputs
            |> List.mapi _factory 
            |> Async.Parallel 
            |> Async.RunSynchronously 
            |> ignore

        assert (counter.Value = expected)
        printfn "totally, counter is increased to %d" counter.Value
 
    let main()=
        test_lock()
