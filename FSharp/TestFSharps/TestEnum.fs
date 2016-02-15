
namespace TestFSharps

open System

open NUnit.Framework
open FsUnit

// Enum can be as simple as just identifier without any values
type Gender = 
    | Male
    | Female

type Color =
    | Red = 0
    | Green = 1
    | Blue = 2

type Direction =
    | Left = 1
    | Top = 2
    | Right = 4
    | Bottom = 8

[<Flags>]
type MyChoices =
    | Nil = 0b00000000
    | Choice1 = 0b00000001
    | Choice2 = 0b00000010
    | All = 0b00000011

[<TestFixture>]
type TestEnum() =

    [<Test>]
    member this.TestDemoUsage() =
        let restroom_tag = function 
            | Male -> "Gentleman"
            | Female -> "Lady" 

        Gender.Male |> restroom_tag |> should equal "Gentleman"
        Gender.Female |> restroom_tag |> should equal "Lady"


    [<Test>]
    member this.TestCast() =
        let c = Color.Blue
        (int c) |> should equal 2

    [<Test>]
    member this.TestToString() =
        Color.Green |> string |> should equal "Green"
        Direction.Right |> string |> should equal "Right"

    [<Test>]
    member this.TestGetName() = 
        // here, when use .NET library, "c" in Color Type is auto-cast to Object
        let c = Color.Red
        Enum.GetName(typeof<Color>,c) |> should equal "Red"

    [<Test>]
    member this.TestMatch() =
        // because F#'s enum can be extended, so you still have to privide _ to work as the default catching
        let fool (color: Color) =
            match color with
                | Color.Red -> "red"
                | Color.Blue -> "blue"
                | Color.Green -> "green"
                | _ -> raise (System.ArgumentOutOfRangeException())

        [Color.Blue;Color.Green; Color.Blue]
            |> List.map fool
            |> should equal ["blue";"green";"blue"]

    [<Test>]
    member this.TestBitFlag()=
        let leftTop = Direction.Left ||| Direction.Top

        (int (leftTop &&& Direction.Left)) |> should equal (int Direction.Left)
        (int (leftTop &&& Direction.Top)) |> should equal (int Direction.Top)

        (int (leftTop &&& Direction.Right)) |> should equal 0
        (int (leftTop &&& Direction.Bottom)) |> should equal 0

    [<Test>]
    member this.TestBitFlag2() =
        let flag = MyChoices.All 
        // when use this flag enum, we always check whether our option is included
        // by comparing "not equal zero"
        flag &&& MyChoices.Choice1 |> should not' (equal MyChoices.Nil)
        flag &&& MyChoices.Choice2 |> should not' (equal MyChoices.Nil)

        // XOR
        let newflag = flag ^^^ MyChoices.Choice1
        newflag &&& MyChoices.Choice1 |> should equal MyChoices.Nil
        newflag &&& MyChoices.Choice2 |> should not' (equal MyChoices.Nil)   

    [<Test>]
    member this.TestParse() =
        Enum.Parse(typeof<Color>,"red",true) |> should equal Color.Red

        (fun() -> Enum.Parse(typeof<Direction>,"x",true) |> ignore) |> should throw typeof<ArgumentException>


        
        
           
        
            