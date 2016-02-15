
namespace PlayAsyncFsharps

open System
open System.Threading

open Helper

module TestProgressEvent =

    type SynchronizationContext with
        member this.RaiseEvent (event : Event<'t>) (args : 't)=
            this.Post( (fun _ -> event.Trigger args), null)

        static member CaptureCurrent()=
            match SynchronizationContext.Current with
                | null -> new SynchronizationContext() // default SynchronizationContext uses thread pool
                | ctxt -> ctxt

    type AsyncGroup<'t> (asyncJobs: seq<Async<'t>>)=
        // *************************** member fields
        let m_allCompleted = new Event<'t[]>()
        let m_jobCompleted = new Event<int*'t>()
        let m_errored = new Event<System.Exception>()
        let m_canceled = new Event<System.OperationCanceledException>()

        let m_cts = new CancellationTokenSource()

        // *************************** event definition
        [<CLIEvent>]
        member this.AllCompleted = m_allCompleted.Publish

        [<CLIEvent>]
        member this.JobCompleted = m_jobCompleted.Publish

        [<CLIEvent>]
        member this.Errored = m_errored.Publish

        [<CLIEvent>]
        member this.Canceled = m_canceled.Publish

        // *************************** public API
        member this.Start()= 
            let syncContext = SynchronizationContext.CaptureCurrent()

            let run = fun index asyncjob -> async {
                let! result = asyncjob
                syncContext.RaiseEvent m_jobCompleted (index,result)
                return result
            }
            let totalwork = asyncJobs |> Seq.mapi run |> Async.Parallel

            Async.StartWithContinuations(
                totalwork,
                (fun results -> syncContext.RaiseEvent m_allCompleted results),
                (fun ex -> syncContext.RaiseEvent m_errored ex),
                (fun oce -> syncContext.RaiseEvent m_canceled oce),
                m_cts.Token)

        member this.Cancel()=
            m_cts.Cancel()

    let main() = 
        let fool seconds = async {
            Thread.Sleep (seconds * 1000)
            return seconds
        }

        let asyncjobs = {1..3} |> Seq.map fool

        let group = new AsyncGroup<int>(asyncjobs)

        // since we runs on non-GUI thread, and the SyncContext will queue action into threadpool
        // so following callbacks will be invoked in threadpool
        group.AllCompleted.Add (fun results -> tprintfn "!!!!! all jobs completed with: %A" results)
        group.JobCompleted.Add (fun (index,result) -> tprintfn "***** %d-th completes with %d" index result)
        group.Start()

        pause()


