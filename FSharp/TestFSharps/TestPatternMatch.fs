namespace TestFSharps

open NUnit.Framework
open FsUnit

module Module4PatternMatch = 
    [<Literal>]
    let Left = 101
    
    [<Literal>]
    let Right = 202
    
    type VerticalDirection = 
        | Up
        | Down

open Module4PatternMatch

[<TestFixture>]
type TestPatternMatch() = 
    
    [<Test>]
    member this.Sample() = 
        let get_title name = 
            match name with
            | "tom" -> "CEO"
            | "mary" | "dick" -> "manager"
            | _ -> "unknown"
        [ "jerry"; "dick"; "tom"; "mary" ]
        |> List.map get_title
        |> should equal [ "unknown"; "manager"; "CEO"; "manager" ]
    
    [<Test>]
    member this.CorrectMatchConstant() = 
        [ 5; 202; 101 ]
        |> List.map (fun n -> 
               match n with
               | Module4PatternMatch.Left -> "left"
               | Module4PatternMatch.Right -> "right"
               | _ -> "unknown")
        |> should equal [ "unknown"; "right"; "left" ]
    
    [<Test>]
    member this.TestValueBinding() = 
        List.init 3 (fun index -> index + 1)
        |> List.map (fun item -> 
               match item with
               | x -> "always matched" // !! chekanote: x here will bound to any input
               | 1 | 2 | 3 -> "never"
               | _ -> "unknown")
        |> should equal (List.init 3 (fun index -> "always matched"))
        let result1 = 
            match 3 with
            | x -> string x
            | 3 -> "never"
            | _ -> "never"
        result1 |> should equal "3"
        // ---------- chekanote: at the right hand-side, it will use captured value
        let x = 100000
        
        let result2 = 
            match 4 with
            | x -> x // bind to input, NOT captured value
            | _ -> -1
        result2 |> should equal 4
    
    [<Test>]
    member this.DemoValueBindingError() = 
        let fool x = 
            match x with
            | Up -> "up"
            | Middle -> "middle"
            | Down -> "down"
        Up
        |> fool
        |> should equal "up"
        // because "Middle" is recognized as a general variable
        // which will be bound to any input
        // so input argument 'Down' is bound to variable 'Middle'
        Down
        |> fool
        |> should equal "middle"
    
    [<Test>]
    member this.TestWhen() = 
        let sign x = 
            // all use "_" here, because we don't care about the structure  
            match x with
            | _ when x < 0 -> -1
            | _ when x > 0 -> 1
            | _ when x = 0 -> 0
            | _ -> raise (System.ArgumentOutOfRangeException("never"))
        [ -3; 0; 67 ]
        |> List.map sign
        |> should equal [ -1; 0; 1 ]
    
    [<Test>]
    member this.TestMatchAny() = 
        let allowAccess userInfo = 
            match userInfo with
            | 1, "x" -> true
            | _, "*8*" -> true
            | 9909, _ -> true
            | _ -> false
        [ (34, "*8*")
          (789, "*8*")
          (9909, "x")
          (9909, "y")
          (1, "x") ]
        |> List.map allowAccess
        |> List.forall (fun item -> item)
        |> should be True
    
    [<Test>]
    member this.TestMatchCollections() = 
        let fool (alist : int list) = 
            match alist with
            | [ 1; 2; 3 ] -> "all matched"
            | [ 1; _ ] -> "two elements start with 1"
            | [ 1; _; _ ] -> "three elements start with 1"
            | _ -> "default"
        [ 5 ]
        |> fool
        |> should equal "default"
        [ 1; 1 ]
        |> fool
        |> should equal "two elements start with 1"
        [ 1; 9; 6 ]
        |> fool
        |> should equal "three elements start with 1"
        [ 1; 2; 3 ]
        |> fool
        |> should equal "all matched"
        let result = 
            match [ 1; 2; 300 ] with
            | [ _; 2; 5 ] -> "no match"
            | [ 1; 2; x ] -> sprintf "match the first two,missing one is %d" x // order matters
            | [ _; 2; 3 ] -> "match the last two"
            | _ -> "default"
        result |> should equal "match the first two,missing one is 300"
    
    [<Test>]
    member this.TestShorthandSyntax() = 
        // note: the argument is neglected, and its type is inferenced
        let get_description = 
            function 
            | File(name) -> sprintf "file<%s>" name
            | Process(name, priority) -> sprintf "process<%s> has priority<%d>" name priority
            | Unknown -> "unknown"
        [ Unknown
          Process("app.exe", 100)
          File("story.txt") ]
        |> List.map get_description
        |> should equal [ "unknown"; "process<app.exe> has priority<100>"; "file<story.txt>" ]
    
    [<Test>]
    member this.TestOrMatch() = 
        let fool item = 
            match item with
            | ("low", value) | ("lo", value) -> sprintf "low=%d" value
            | (("hi" | "high"), value) -> sprintf "high=%d" value
            | _ -> failwith "unrecognized"
        ("high", 100)
        |> fool
        |> should equal "high=100"
        ("lo", 9)
        |> fool
        |> should equal "low=9"
    
    [<Test>]
    member this.TestMatchFailException() = 
        let fool x = 
            match x with
            | 1 -> "1"
            | 100 -> "100"
        (fun () -> fool 2 |> ignore) |> should throw typeof<MatchFailureException>
    
    [<Test>]
    member this.TestMatchType() = 
        let fool (o : obj) = 
            match o with
            | :? int -> "int"
            | :? single -> "single"
            | :? double -> "double"
            | :? string -> "string"
            | _ -> "others"
        [ box (88)
          box (99.9)
          box ("stasi")
          box (66.6f) ]
        |> Seq.map fool
        |> should equal [ "int"; "double"; "string"; "single" ]
    
    [<Test>]
    member this.TestOr() = 
        let detectZero point = 
            match point with
            | (0, _) | (_, 0) -> true
            | _ -> false
        [ (0, 0)
          (1, 2)
          (0, 6)
          (9, 0) ]
        |> Seq.map detectZero
        |> should equal [ true; false; true; true ]
    
    [<Test>]
    member this.TestAnd() = 
        let detectOnlyZero point = 
            match point with
            | (0, 0) -> None
            | (x, y) & (0, _) -> Some(y)
            | (x, y) & (_, 0) -> Some(x)
            | _ -> None
        [ (6, 8)
          (0, 0)
          (0, 9)
          (6, 0) ]
        |> Seq.map detectOnlyZero
        |> should equal [ None
                          None
                          Some(9)
                          Some(6) ]
