
namespace TestAdvDotNet

module TestFileStream =

    open System
    open System.IO

    type FileStreamConfig = {
        KernelBufSize : int
        AppBufSize: int
    }

    let private checkRead (config: FileStreamConfig) =
        async {
            let filename = "testdata.bin"
            use fstream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None, config.KernelBufSize, true)

            let buffer = Array.zeroCreate<byte> config.AppBufSize

            let total = ref 0
            let counter = ref 0
            let readed = ref config.AppBufSize
            while !readed > 0 do
                let! n = fstream.AsyncRead(buffer)
                readed := n
                incr counter
                total := !total + n
                printfn "[%3d] %3d bytes readed" !counter n

                // since it is FileStream, not SocketStream
                // the case "readed < expected"
                // can only happen once
                // that is when the EOF is met
                // so when readed < expected
                // then the next read must return zero
                if n < config.AppBufSize then
                    printfn "!!! Readed < Expected"
                    let! n = fstream.AsyncRead buffer
                    assert (n = 0)
                    printfn "!!! EOF"
                    readed := 0

            printfn "totally %3.2f M bytes read" <| (float !total)/1048576.0
        }
        |> Async.RunSynchronously

    let private testRead_KernelBuf_GT_AppBuf() =
        {KernelBufSize = 8192;AppBufSize = 1024}
        |> checkRead

    let private testRead_AppBuf_GT_KernelBuf() =
        {KernelBufSize = 1024;AppBufSize = 8192}
        |> checkRead

    let main() =
        testRead_KernelBuf_GT_AppBuf()
        // testRead_AppBuf_GT_KernelBuf()



