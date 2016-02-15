
namespace PlayAsyncFsharps

open System.IO

module TestBasicIO =
    /// test use "use" in seq 
    /// which will make the stream being captured, whose lifecycle will be extended beyond the seq scope
    /// which will make the stream disposed when the iteration is completed
    let test_use_inseq()=
        seq {
            use reader = File.OpenText "text.txt"
            while not reader.EndOfStream do
                yield reader.ReadLine()
        } |> Seq.iteri (fun index item -> printfn "[%-2d] %s" (index+1) item)

    let main()=
        test_use_inseq()
