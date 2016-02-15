
#light

namespace TestFSharps.Records

open NUnit.Framework
open FsUnit

type Movie = {
    Title : string;
    Year : int;
    Actors : string[];
    mutable Score: int
    }

/// between the fields, ";" is optional
type Employee =
    {
        Id : int
        Rank : string
        Name : string
    }
    member this.Description = sprintf "%s(%s)" this.Name this.Rank

type Dot = {X:int; Y:int}
type Point = {X:float; Y:float}

[<TestFixture>]
type TestRecords()=

    [<Test>]
    member this.Sample() =
        // !!! if you have defined the record before, you don't need to specify the type when
        // !!! initiating an instance of that type, you can left compiler to inference its type
        let movie = {
            Title="test";
            Year=2008;
            Actors=[|"Tom"|];
            Score = 4}
        movie.Title |> should equal "test"
        movie.Year |> should equal 2008

        movie.Score <- 5
        movie.Score |> should equal 5

        // !!! chekanote: explicit specifying the type by suffix ": RecordTypeName"
        let employee = {Id=1;Name="mary";Rank="manager"} : Employee
        employee.Id |> should equal 1

    [<Test>]
    member this.TestSameFieldsRecords()=
        // below codes has compiling error, because "latest definition" wins
        // so it is inferred as Point, not Dot
        // let temp = {X=1;Y=2}
        let dot1 = {X=1;Y=2} : Dot
        let dot2 = {Dot.X = 1; Y=2}

        // it is inferred as Point by default. because Point is defined after Dot
        let pnt = {X=1.0; Y= 2.0}
        pnt.GetType() |> should equal typeof<Point>

    [<Test>]
    member this.TestDecompose()=
        let movie = {
            Title="test";
            Year=2008;
            Actors=[|"Tom"|];
            Score = 4}

        let {Title=title;Year=year;Actors=actors;Score=score} = movie

        title |> should equal "test"
        year |> should equal 2008
        actors |> should equal ["Tom"]
        score |> should equal 4

    [<Test>]
    member this.TestReferenceTypeFeature()=
        let add_score (m:Movie) (n:int) =
            m.Score <- m.Score + n

        let movie = {
            Title="test";
            Year=2008;
            Actors=[|"Tom"|];
            Score = 5}
        add_score movie 6

        movie.Score |> should equal 11

    [<Test>]
    member this.TestCopyAndUpdate()=
        let employee1 = {Rank="manager";Id=2;Name="tom"}
        let employee2 = {employee1 with Id=5;Name="mary"}

        employee2.Id |> should equal 5
        employee2.Rank |> should equal employee1.Rank

    /// records are provided automatically "value-based equality comparison"
    [<Test>]
    member this.TestEqual()=
        let employee1 = {Rank="manager";Id=2;Name="tom"}
        let employee2 = {Id=2;Name="tom";Rank="manager"}

        employee1 |> should not' (be sameAs employee2)
        employee1 |> should equal employee2
        (employee1 = employee2) |> should equal true
        employee1.Equals(employee2) |> should equal true

        let employee3 = {employee1 with Id=10}
        employee3 |> should not' (equal employee2)

    [<Test>]
    member this.TestMember()=
        let employee1 = {Rank="manager";Id=2;Name="tom"}
        employee1.Description |> should equal "tom(manager)"

    [<Test>]
    member this.TestMatch() =
        let evaluate employee =
            match employee with 
                | {Rank="manager";Id=2} -> sprintf "%s is excellent" employee.Name
                | {Rank="manager"} -> sprintf "%s is ordinary" employee.Name
                | {Rank="CEO";Name="dick"} -> sprintf "%s is bad" employee.Name
                | {Rank="CEO"} -> sprintf "%s is good" employee.Name
                | _ -> "unknown"

        {Rank="manager";Id=2;Name="tom"} |> evaluate |> should equal "tom is excellent"
        {Rank="CEO";Id=2;Name="mary"} |> evaluate |> should equal "mary is good"
        {Rank="manager";Id=20;Name="jerry"} |> evaluate |> should equal "jerry is ordinary"
        {Rank="CEO";Id=20;Name="dick"} |> evaluate |> should equal "dick is bad"



