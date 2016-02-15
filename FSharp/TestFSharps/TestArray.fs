
namespace TestFSharps

open NUnit.Framework
open FsUnit

[<TestFixture>]
type TestArray() = 
    
    [<Test>]
    member this.SimpleTest() =
        let intArray = [|1;2;3|]
        intArray.[0] |> should equal 1
        intArray.Length |> should equal 3

        let strArray = [|"hello";"wsu"|]
        strArray.[1] |> should equal "wsu"
        strArray.Length |> should equal 2

    [<Test>]
    member this.TestCreateByRange() = 
        let array = [|1..3|]
        CollectionAssert.AreEqual([|1;2;3|],array)

        let array = [|1..2..4|]
        CollectionAssert.AreEqual([|1;3|],array)

        let array = [|5..-2..1|]
        CollectionAssert.AreEqual([|5;3;1|],array)

    [<Test>]
    member this.TestZeroCreate() = 
        // note: when "zeroCreate" is created, array's type is not fixed
        let array = Array.zeroCreate 3
        // note: only when the first element is assigned, array's type now is fixed
        // and after that, it can only accept one specific type, cannot accept other types anymore 
        array.[1] <- "b"
        CollectionAssert.AreEqual([|null;"b";null|],array)

        let array = Array.zeroCreate 2
        for index = 0 to array.Length - 1 do
            array.[index] <- ((index + 1) * 2)
        CollectionAssert.AreEqual([|2;4|],array)

    [<Test>]
    member this.TestCreate()=
        let names = Array.create 3 "x"
        names |> should equal [|"x";"x";"x"|]

        Array.set names 1 "Y"
        (Array.get names 1) |> should equal "Y" 

    [<Test>]
    member this.TestExplicitType() =
        let arr1: string[] = [||]
        let arr2: int array = Array.zeroCreate 2
        
        let random = new System.Random()
        let arr3  = Array.zeroCreate<double> 3
        for index = 0 to arr3.Length-1 do
            arr3.[index] <- random.NextDouble()

    [<Test>]
    member this.TestSlice()=
        let original = [|'a';'b';'c';'d';'z'|]

        let sliced = original.[0..2]
        sliced |> should equal [|'a';'b';'c'|]

        // ------------------ you cannot slice as in numpy, because F# cannot slice by using a indice array
        // ------------------ codes below won't pass the compilation
        // let indices = [|0;3;4|]
        // let impossible = original.[ indices ]
        
        // ------------------ sliced generats fresh array, isolated from the orginal array
        sliced.[0] <- 'x'
        sliced |> should equal ['x';'b';'c']
        original |> should equal [|'a';'b';'c';'d';'z'|] 

        original.[..1] |> should equal [|'a';'b'|]
        original.[2..] |> should equal ['c';'d';'z']

        let copy = original.[0..]
        copy |> should equal original
        copy |> should not' (be sameAs original)

        // ---------- by using function 
        let start = 0
        let length = 2
        // the second argument is not EndIndex, but length
        (Array.sub original start length) |> should equal [|'a';'b'|]


    /// Array is reference type, "let y = x" making these two symbols point to the same value
    /// modification through one symbol will reflect on another, vice versa
    [<Test>]
    member this.TestReferenceTypeFeature()=
        let x = [|1;2;3|]
        let y = x
        y |> should be (sameAs x)

        x.[0] <- 99
        y |> should equal [|99;2;3|]

        y.[2] <- 888
        x |> should equal [|99;2;888|]

    [<Test>]
    member this.TestArrayOfArray()=
        // ------------------------ rectangle matrix
        let rectAngle = [|
            [|1;2;3|];
            [|4;5;6|]
        |]
        rectAngle.[1].[1] |> should equal 5
        rectAngle.Rank |> should equal 1

        // ------------------------ jagged matrix
        let jagged = [|
            [|1;2|];
            [|3;4;5;6|]|]
        jagged.[1].[2] |> should equal 5

    [<Test>]
    member this.TestIndexOverRangeException()=
        let array = Array.zeroCreate 3
        (fun()-> array.[3] <- 5) |> should throw typeof<System.IndexOutOfRangeException> 

        // unlike some functional programming language like python
        // F# doesn't allow negative index
        (fun() -> array.[-1] |> ignore) |> should throw typeof<System.IndexOutOfRangeException>

    [<Test>]
    member this.TestArray2DCreate()=
        let matrix = Array2D.zeroCreate<int> 2 3
        matrix.GetLength(0) |> should equal 2
        matrix.GetLength(1) |> should equal 3
        matrix.Rank |> should equal 2

        // matrix's element are initialized by zero
        // and element in the matrix can be accessed by .[row,col], other than .[row].[col]
        matrix.[1,1] <- 100
        matrix.[1,0] |> should equal 0
        matrix.[1,1] |> should equal 100

        // ------------------ initialize to a specific value
        let numRow = 2
        let numCol = 4
        let matrix = Array2D.create<int> numRow numCol 88

        for row = 0 to numRow-1 do
            for col = 0 to numCol-1 do
                matrix.[row,col] |> should equal 88

    [<Test>]
    member this.TestInit()=
        let seed = 3
        let array = Array.init 3 (fun index ->
                                            let num = index + seed
                                            num * num)
        array |> should equal [|9;16;25|]

    [<Test>]
    member this.TestMap() =
        Array.init 3 (fun index->index + 1)
            |> Array.map (fun num->num* num)
            |> should equal [|1;4;9|]

        [|-10.0;0.0;88.88|]
            |> Array.map System.Math.Sign
            |> should equal [|-1;0;1|]

        Array.map2 (fun a b->a+b) [|1;2|] [|3;4;|]
            |> should equal [|4;6|]

        Array.init 4 (fun index->index + 1)
            |> Array.mapi (fun index n->if index%2 = 0 then n else -n)
            |> should equal [|1;-2;3;-4|]

        Array.mapi2 (fun index a b->sprintf "%d-%d-%d" index a b) [|1;2|] [|3;4|]
            |> should equal [|"0-1-3";"1-2-4"|]

    [<Test>]
    member this.TestFold() =
        Array.init 3 (fun index -> index + 1)
            |> Array.fold (fun acc num->acc+ num) 10
            |> should equal 16

        let sb = [|"Hello";"wsu";"from";"F#"|]
                |> Array.fold 
                    (fun (acc:System.Text.StringBuilder)  (element: string) ->  acc.Append(element+" ")) 
                    (new System.Text.StringBuilder())
        (sb.ToString()) |> should equal "Hello wsu from F# "

    [<Test>]
    member this.TestReduce()=
        // this difference between Fold and Reduce is "reduce" don't need seed
        // the seed is always the element at [0], and accumulator and element must have the same type
        Array.init 3 (fun index -> index + 1)
            |> Array.reduce (fun acc element-> acc+ element)
            |> should equal 6

    [<Test>]
    member this.TestScan()=
        Array.init 3 (fun index -> index)
            |> Array.scan (fun acc item -> acc + item) 0
            |> should equal [|0;0;1;3|]

    [<Test>]
    member this.TestFilter() =
        Array.init 4 (fun index -> index + 1) 
            |> Array.filter (fun item -> item%2 =0)
            |> should equal [|2;4|]

        [|"hello";"wsu";"from";"cheka"|]
            |> Array.filter (fun item -> item.Length = 3)
            |> should equal [|"wsu"|]

    [<Test>]
    member this.TestZip() = 
        let array1 = [|1;2|]
        let array2 = [|"hello";"wsu"|]
        
        Array.zip array1 array2
            |> should equal [|(1,"hello");(2,"wsu")|]

    [<Test>]
    member this.TestCapturedInClosure() =
        let counter = [|0|]

        let funcWithSideEffect() = 
            let currentValue = counter.[0]
            counter.[0] <- currentValue + 1
            currentValue

        funcWithSideEffect() |> should equal 0
        funcWithSideEffect() |> should equal 1
        funcWithSideEffect() |> should equal 2

            







                

        





