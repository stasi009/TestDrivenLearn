
#light

namespace TestFSharps.Classes

open NUnit.Framework
open FsUnit

type MyArray<'t>(m_array: 't[]) =

    member this.Item
        with get index = 
            if index >= 0 && index < m_array.Length then m_array.[index]
            else failwith "index over range"
        and set index value = 
            if index >= 0 && index < m_array.Length then m_array.[index] <- value
            else failwith "index over range"

type NonGenericArray(m_array: 't[]) =
    member this.Item
        with get index = 
            if index >= 0 && index < m_array.Length then m_array.[index]
            else failwith "index over range"
        and set index value = 
            if index >= 0 && index < m_array.Length then m_array.[index] <- value
            else failwith "index over range"

// another way to express generic types, put 't before the name
type 't MyMatrix(numRows:int,numCols:int,initFactory: int->int->'t)=
    let m_table = Array2D.zeroCreate<'t> numRows numCols
    do 
        for row = 0 to numRows - 1 do
            for col = 0 to numCols - 1 do
                m_table.[row,col] <- (initFactory row col)

    member this.Item
        with get (row,col) = m_table.[row,col]
        and set (row,col) value = m_table.[row,col] <- value

// put the constraints that the item in this list must have default constructor
type MyList<'t when 't :(new:unit->'t)>()=
    let m_list = new System.Collections.Generic.List<'t>()

    member this.Length with get() = m_list.Count

    member this.AddOne() =
        m_list.Add(new 't())

module Module4GenericClasses=
    type Fool()=
        class
        end

    let my_concat (collections : seq<seq<'t>>)=
        let alist = new System.Collections.Generic.List<'t>()
        collections |> Seq.iter (fun items->
            items |> Seq.iter (fun item -> alist.Add item))
        alist

[<TestFixture>]
type TestGenericClasses()=

    [<Test>]
    member this.TestIndexProperty()=
        let collection = new MyArray<int>([|99;88;|])
        collection.[0] |> should equal 99

        collection.[0] <- 100
        collection.[0] |> should equal 100

        // ------------------ test exceptions
        (fun() -> collection.[100] |> ignore) |> should throw typeof<System.Exception>

    [<Test>]
    member this.TestRealGeneric()=
        let intarray = new MyArray<int>([|1;2|])
        let strarray = new MyArray<string>([|"hello";"generics"|])
        // chekanote: !!! we have to explicit specify the type, otherwise it will cause a compiling-error
        // XXX let strarray = new MyArray([|"hello";"generics"|])
        ()

    [<Test>]
    member this.TestNonGenerics()=
        // let strArray = new NonGenericArray([|"hello";"f#"|])
        let intArray1 = new NonGenericArray([|99;88;|])
        let intArray2 = new NonGenericArray([|101;|])

        // below codes will cause compile error, the type will fixed into the type being first used
        // so it can be only one type
        // let strArray = new NonGenericArray([|"hello";"f#"|])
        ()

    [<Test>]
    member this.TestMultiIndexProperty()=
        let matrix = new MyMatrix<string>(2,3,(fun row col-> sprintf "%d * %d = %d" row col (row*col)))
        matrix.[1,2] |> should equal "1 * 2 = 2"

        matrix.[1,1] <- "stasi"
        matrix.[1,1] |> should equal "stasi"

    [<Test>]
    member this.TestDefconstructorConstraints()=
        let alist = new MyList<Module4GenericClasses.Fool>()

        for index = 0 to 9 do
            alist.AddOne()

        alist.Length |> should equal 10

    [<Test>]
    member this.TestFlexibleTypeAsReturnType()= 
        let __form_string (numbers: int seq) =
            let sb = System.Text.StringBuilder()
            numbers |> Seq.iter (fun item -> sb.Append(string item) |> ignore)
            sb.ToString()

        // if don't use #, you have to explicit cast, to make the return type compatible
        let form_string (factory: unit->seq<int>) =
            factory() |> __form_string
        (fun()->[1;2]:>seq<int>) |> form_string  |> should equal "12"

        let form_string_flexible (factory: unit->#seq<int>) = 
            factory() |> __form_string
        (fun()->[1;2]) |> form_string_flexible |> should equal "12"

    [<Test>]
    member this.TestNoNeedUseFlexibleType()=
        let array = [|1;2|]
        let list = [5;6;7]
        (Module4GenericClasses.my_concat [list;array;]) |> should equal [5;6;7;1;2;]
        (Module4GenericClasses.my_concat [["hello";"f#"];[|"wsu"|];]) |> should equal ["hello";"f#";"wsu"]

    