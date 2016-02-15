
namespace TestFSharps

[<Sealed>]
type TestClassInteroperate()=
    member this.Add x y =  x + y
    member this.AddwithTuple (x,y) = x + y

