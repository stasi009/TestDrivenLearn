
namespace TestFSharps

open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestRecursion()=
    [<Test>]
    member this.SampleRecursion()=
        let rec factorial n =
            if n <= 1 then 1
            else n * (factorial (n-1))
        factorial 5 |> should equal 120

    [<Test>]
    member this.TestMutualRecursion()=
        let rec IsEven x =
            if x = 0 then true
            elif x = 1 then false
            else IsOdd (x - 1)
        and IsOdd x =
            if x = 0 then false
            elif x = 1 then true
            else IsEven (x - 1)

        (IsEven 5) |> should be False 
        (IsEven 6) |> should be True

        (IsOdd 5) |> should be True 
        (IsOdd 6) |> should be False

    /// Tail Recursion: the last instruction executed in the method is the recursive call
    /// there is no "pending operation" for the runtime to worry about
    /// tail-recursive functions can call itself indefinitely without consuming stack space.
    [<Test>]
    member this.TestTailRecursion1()=
        let rec private_factorial n result =   
            if n <= 1 then result
            else private_factorial (n - 1) (n * result)

        let fact n = 
            private_factorial n 1

        (fact 5) |> should equal 120

    [<Test>]
    member this.TestTailRecursion2()=
        let rec fool n (x : 't) (accumulator : 't list) =
            if n <= 0 then accumulator
            else fool (n-1) x (x::accumulator)

        let fool_helper n x =
            fool n x []

        (fool_helper 3 1 ) |> should equal [1;1;1]
        (fool_helper 4 "x") |> should equal ["x";"x";"x";"x"]

    [<Test>]
    member this.TestReverseListBeforeReturn()=
        let rec fool_map mapper inputlist accumulator =
            match inputlist with
                | [] -> List.rev accumulator
                | header::tail -> fool_map mapper tail ((mapper header)::accumulator)

        let fool_map_helper mapper inputlist = fool_map mapper inputlist []

        [2;3] |> fool_map_helper (fun n->n*n) |> should equal [4;9]
        ["cheka";"wsu"] |> fool_map_helper (fun s->s.Length) |> should equal [5;3]






        
