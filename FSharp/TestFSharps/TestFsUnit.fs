namespace TestFSharps

open NUnit.Framework
open FsUnit

// there are two "Assert" classes defined
// one is in NUnit.Framework, the other is in FsUnit
// both classes provide "AreEqual" methods but with different signature, which cause "naming clash"
// however, F# doesn't support aliasing the namespace
// so I have to create an alias class for NUnit.Framework.Assert
type NuAssert = NUnit.Framework.Assert

[<TestFixture>]
type TestFsUnit() = 
    
    [<Test>]
    member this.TestShouldBe() = 
        true |> should be True
        false |> should be False
        true |> should not' (be False)
        false |> should not' (be True)
        null |> should be Null
    
    [<Test>]
    member this.TestSameAs() = 
        let o1 = new obj()
        let o2 = o1
        let o3 = new obj()
        o1 |> should be (sameAs o2)
        o1 |> should not' (be (sameAs o3))
    
    [<Test>]
    member this.TestGreatLessThan() = 
        11 |> should be (greaterThan 10)
        11.1 |> should be (greaterThan 10.8)
        9 |> should not' (be greaterThan 10)
        11 |> should be (greaterThanOrEqualTo 10)
        9 |> should not' (be greaterThanOrEqualTo 10)
        10 |> should be (lessThan 11)
        10 |> should not' (be lessThan 9)
        10.0 |> should be (lessThanOrEqualTo 10.1)
        10.1 |> should not' (be lessThanOrEqualTo 9.9)
    
    [<Test>]
    member this.TestType() = 
        3.14 |> should be ofExactType<float>
        3.14f |> should be ofExactType<float32>
        "stasi" |> should be ofExactType<string>
        "stasi" |> should not' (be ofExactType<obj>)
        "stasi" |> should be instanceOfType<string>
        "stasi" |> should be instanceOfType<obj>
    
    [<Test>]
    member this.TestShouldEqual() = 
        10 |> should equal 10
        1 |> should not' (equal 0)
        let x = 4
        x |> should equal 4
        x |> should not' (equal 6)
    
    (*the equal is based on seq<int>         
    so different collection, such as, array and list        
    can "equal" with each other, if the length and each element are equal*)
    [<Test>]
    member this.TestCollectionEqual() = 
        [ 1; 2; 3 ] |> should equal [| 1; 2; 3 |]
        [| 2..2..7 |] |> should equal [ 2; 4; 6 ]
        { 1..3 } |> should equal [| 1; 2; 3 |]
        [| 1; 2; 3 |] |> should not' (equal [| 3; 2; 1 |])
        [| 1; 2; 3 |] |> should not' (equal [| 2; 1 |])
    
    [<Test>]
    member this.TestContain() = 
        let array = [ 1..3 ]
        array |> should contain 3
        array |> should not' (contain 4)
        let b = [| 2..2..7 |]
        b |> should contain 4
        b |> should not' (contain 8)
    
    // test "almost equal" by syntax provided by NUnit
    [<Test>]
    member this.TestAlmostEqualByNunit() = 
        NuAssert.AreEqual(3.14, 3.139999999999, 1e-3)
        let x = 3.14
        let y = x + 1e-4
        NuAssert.AreEqual(x, y, 1e-3)
        Assert.AreNotEqual(x, y)
    
    [<Test>]
    member this.TestEqualWithin() = 
        8.1 |> should (equalWithin 0.1) 8.15
        8.1 |> should not' ((equalWithin 0.01) 8.15)
    
    [<Test>]
    member this.TestString() = 
        "cheka" |> should startWith "ch"
        "cheka" |> should not' (startWith "x")
        "cheka" |> should endWith "eka"
        "cheka" |> should not' (endWith "y")
        "" |> should be EmptyString
        "" |> should be NullOrEmptyString
        null |> should be NullOrEmptyString
    
    [<Test>]
    member this.TestThrowException() = 
        (fun () -> failwith "test" |> ignore) |> should throw typeof<System.Exception>
        let ex = Assert.Throws<System.Exception>(fun () -> failwith "test message" |> ignore)
        ex.Message |> should equal "test message"
    
    // can work without NUnit as a standalone application
    static member public TestMain() = 
        let tester = new TestFsUnit()
        tester.TestShouldEqual()
        tester.TestAlmostEqualByNunit()
        tester.TestCollectionEqual()
        tester.TestContain()
        printfn "!!!!!! all tests done"
