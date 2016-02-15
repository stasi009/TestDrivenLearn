
namespace TestFSharps.Classes

open NUnit.Framework
open FsUnit

type DisposeFlag()=
    class

    let mutable m_disposed = false

    member this.IsDisposed 
        with get() = m_disposed
        and set newvalue = m_disposed <- newvalue

    end

type MyDisposable(m_flag : DisposeFlag)=
    class
        interface System.IDisposable with
            member this.Dispose() = m_flag.IsDisposed <- true
    end

[<TestFixture>]
type TestDispose()=
    class

    [<Test>]
    member this.TestUse()=
        let fool flag =
            use resource = new MyDisposable(flag)
            resource |> ignore // resource's Dispose will be invoked when it is out of scope

        let flag = new DisposeFlag()
        flag.IsDisposed |> should be False

        fool flag
        flag.IsDisposed |> should be True

    [<Test>]
    member this.TestUsing()=
        let fool num (flag:DisposeFlag) = 
            using (new MyDisposable(flag)) (fun resource->
                flag.IsDisposed |> should be False// resource will only be disposed at the end of this function
                num)

        let flag1 = new DisposeFlag()
        (fool 1 flag1) |> should equal 1
        flag1.IsDisposed |> should be True

        let flag2 = new DisposeFlag()
        (fool "f#" flag2) |> should equal "f#"
        flag2.IsDisposed |> should be True

    end