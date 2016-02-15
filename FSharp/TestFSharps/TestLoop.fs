namespace TestFSharps

open System
open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestLoop() = 
    [<Test>]
    member this.TestDownto() = 
        seq { 
            for index = 5 to 2 do
                yield index
        }
        |> should equal []
        seq { 
            for index = 5 downto 2 do
                yield index
        }
        |> should equal [ 5; 4; 3; 2 ]
