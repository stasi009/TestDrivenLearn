
namespace TestFSharps

open NUnit.Framework
open FsUnit

module Module4ActivePattern =
    let is_multiple n basenumber = if n % basenumber = 0 then Some(n) else None
    let (|Even|_|) n = is_multiple n 2
    let (|MultipleOf3|_|) n = is_multiple n 3
    let (|MultipleOf5|_|) n = is_multiple n 5

[<TestFixture>]
type TestActivePatterns()=

    [<Test>]
    member this.TestSingleIdentifier()=
        // note: active pattern is just like functions called in pattern matching
        // just a special function, because regular function cannot be called in the pattern matching
        // beside defined using that ugly (||), its name must begin with uppercase letter
        let (|Int2Str|) (n : int) = string n

        let fool n =
            match n with 
                | Int2Str s when s.Length = 2 -> "10~99"
                | Int2Str s when s.Length = 3 -> "100~999"
                | _ -> "unknown"

        [99;1;897;45]   |> List.map fool 
                        |> should equal ["10~99";"unknown";"100~999";"10~99"]


    [<Test>]
    member this.TestMultiIdentifier()=
        let (|Child|Teen|Adult|Senior|) (age:int) =
            if age < 13 then Child
            elif age >= 13 && age < 18 then Teen
            elif age >= 18 && age < 60 then Adult
            else Senior

        let participate (age:int) =
            match age with 
                | Child -> "child"
                | Teen -> "teen"
                | Adult -> "adult"
                | Senior -> "senior"

        [1;48;88;34;15] |> List.map participate
                        |> should equal ["child";"adult";"senior";"adult";"teen"]

    [<Test>]
    member this.TestOption_ForOr() = 
        let or_qualified n=
            match n with
                | Module4ActivePattern.Even x -> "m2"
                | Module4ActivePattern.MultipleOf3 x -> "m3"
                | Module4ActivePattern.MultipleOf5 x -> "m5"
                | _ -> "no"

        [2;3;10;5;7;6;] |> List.map or_qualified
                    |> should equal ["m2";"m3";"m2";"m5";"no";"m2"]

    [<Test>]
    member this.TestOption_ForAnd() =
        let and_qualified n =
            match n with
                | (Module4ActivePattern.Even a) & (Module4ActivePattern.MultipleOf3 b) & (Module4ActivePattern.MultipleOf5 c)-> true
                | _ -> false

        [1;3;60;2;30;] |> List.map and_qualified
                    |> should equal [false;false;true;false;true;]

    [<Test>]
    member this.TestParameterized() =
        let (|Matches|_|) (target: string) (mystring: string) =
            if mystring.Contains(target) then Some(true) else None

        let checkContainAndPrint (target: string) (mystring: string) =
            match mystring with
                | Matches target result -> sprintf "'%s' contains '%s'" mystring target
                | _ -> "Not Contain"

        (checkContainAndPrint "ch" "cheka") |> should equal "'cheka' contains 'ch'"
        (checkContainAndPrint "x" "cheka") |> should equal "Not Contain"




