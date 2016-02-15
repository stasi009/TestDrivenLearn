namespace TestAdvDotNet

open System
open NUnit.Framework
open FsUnit

module Module4StructureEqualCompare = 
    type Person = 
        { Id : int
          Name : string }
    
    // below is a record, but it cannot have "structural equality"
    // because it has function as its member fields, and function doesn't support 'eqaulity'
    // so we must override by ourselves
    [<CustomEquality; CustomComparison>]
    type MyThing = 
        { Stamp : int
          Behaviour : int -> int // prevent the default "structural equality"
                                 }
        
        override x.Equals yobj = 
            match yobj with
            | :? MyThing as y -> (x.Stamp = y.Stamp)
            | _ -> false
        
        override x.GetHashCode() = hash x.Stamp
        interface IComparable with
            member x.CompareTo yobj = 
                match yobj with
                | :? MyThing as y -> compare x.Stamp y.Stamp
                | _ -> invalidArg "yobj" "cannot compare values of different types"
    
    type MyBox<[<EqualityConditionalOn; ComparisonConditionalOn>] 'T>(value : 'T) = 
        member x.Value = value
        
        override x.Equals(yobj) = 
            match yobj with
            | :? MyBox<'T> as y -> Unchecked.equals x.Value y.Value
            | _ -> false
        
        override x.GetHashCode() = Unchecked.hash x.Value
        interface System.IComparable with
            member x.CompareTo yobj = 
                match yobj with
                | :? MyBox<'T> as y -> Unchecked.compare x.Value y.Value
                | _ -> invalidArg "yobj" "cannot compare values of different types"

open Module4StructureEqualCompare

[<TestFixture>]
type TestStructureEqualCompare() = 
    
    [<Test>]
    member this.TestEqual() = 
        [ // list supports structural equality
          1; 2 ] = [ 1; 2 ] |> should be True
        [| // array supports structural equality
           "cheka"; "stasi" |] = [| "cheka"; "stasi" |] |> should be True
        (// tuple supports structural equality
         689, "stasi") = (689, "stasi") |> should be True
        { // record supports structural equality
          Id = 1
          Name = "cheka" } = { Id = 1
                               Name = "cheka" }
        |> should be True
    
    [<Test>]
    member this.TestCustomEqualCompare() = 
        let t1 = 
            { Stamp = 9
              Behaviour = fun x -> x }
        
        let t2 = 
            { Stamp = 9
              Behaviour = fun x -> x - 1 }
        
        let t3 = 
            { Stamp = 8
              Behaviour = fun x -> x * x }
        
        t1 |> should not' (be sameAs t2)
        t1 = t2 |> should be True
        t1 > t3 |> should be True
    
    [<Test>]
    member this.TestConditionOn() = 
        // ------------ supportable type
        let box1 = new MyBox<int>(9)
        let box2 = new MyBox<int>(10)
        let box3 = new MyBox<int>(9)
        box1 = box3 |> should be True
        box1 < box2 |> should be True
        // ------------ !!! TYPE NOT SUPPORT
        let box4 = new MyBox<int -> int>(fun x -> x)
        let box5 = new MyBox<int -> int>(fun x -> x * x)
        ()
// !!!!!!!!! below codes CANNOT compile
// !!!!!!!!! because function cannot check equality
// box4 = box5 |> should be False
