
namespace TestFSharps

open NUnit.Framework
open FsUnit

module Module4OverloadOperators=
    type Vector =
        {
            X : int
            Y : int
        }
        static member (+) (left : Vector, right : Vector) = 
            {X = left.X + right.X; Y=left.Y + right.Y}

        static member (-) (left : Vector, right : Vector) =
            {X = left.X - right.X; Y = left.Y - right.Y}
open Module4OverloadOperators

[<TestFixture>]
type TestOverloadOperator()=

    [<Test>]
    /// overloaded operators which are related only to specific type
    member this.Test4SpecificType()=
        let v1 = {X = 50;Y=6}
        let v2 = {X = 5; Y = 60}
        (v1 + v2) |> should equal {X = 55;Y = 66}
        (v1-v2) |> should equal {X=45; Y= -54}
        (v2-v1) |> should equal {X= -45;Y=54}

