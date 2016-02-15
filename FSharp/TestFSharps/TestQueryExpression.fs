namespace TestFSharps

open System
open NUnit.Framework
open FsUnit

module TestQueryExpressionModule = 
    type MenuItem = 
        { Name : string
          Price : float }

open TestQueryExpressionModule

[<TestFixture>]
type TestQueryExpression() = 
    
    let menu = 
        [ { Name = "soup"
            Price = 6.75 }
          { Name = "sandwich"
            Price = 15.50 }
          { Name = "salad"
            Price = 12.95 }
          { Name = "fruit"
            Price = 4.25 }
          { Name = "cake"
            Price = 5.00 }
          { Name = "soda"
            Price = 3.50 }
          { Name = "water"
            Price = 2.00 } ]
    
    [<Test>]
    member this.Sample1() = 
        query { 
            for m in menu do
                select m.Name
        }
        |> should equal [| "soup"; "sandwich"; "salad"; "fruit"; "cake"; "soda"; "water" |]
        query { 
            for m in menu do
                where (m.Price < 10.0)
                select m.Name
        }
        |> should equal [| "soup"; "fruit"; "cake"; "soda"; "water" |]
        query { 
            for m in menu do
                sortBy m.Price
                select m.Name
        }
        |> should equal [| "water"; "soda"; "fruit"; "cake"; "soup"; "salad"; "sandwich" |]
    
    [<Test>]
    member this.TestContains() = 
        query { 
            for m in menu do
                select m.Name
                contains "soup"
        }
        |> should be True
        query { 
            for m in menu do
                select m.Name
                contains "NotExisted"
        }
        |> should be False
    
    [<Test>]
    member this.TestCount() = 
        query { 
            for m in menu do
                where (m.Price < 10.0)
                select m.Name
                count
        }
        |> should equal 5
