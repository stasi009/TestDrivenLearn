
namespace PlayAsyncFsharps.Agent

open PlayAsyncFsharps.Helper

module AgentReply =
    let private test_sync_reply()=
        use agent = new CounterAgent()

        agent.Increment 100
        agent.Increment 11

        printfn "current sum = %d" (agent.Fetch())

        agent.Increment 555
        printfn "current sum = %d" (agent.Fetch())

    let private test_async_reply()=
        use agent = new CounterAgent()

        let fetch() =
            let asyncfetch = agent.AsyncFetch()
            Async.StartWithContinuations(asyncfetch,
                (fun num->tprintfn "result=%d got" num),
                ignore,
                ignore)

        agent.Increment 100
        agent.Increment 11
        fetch()

        agent.Increment 777
        fetch()

        pause()


    let main()=
        // test_sync_reply()
        test_async_reply()

