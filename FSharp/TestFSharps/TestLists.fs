
namespace TestFSharps

open NUnit.Framework
open FsUnit

module Module4TestLists =
    let test_iter() =
        let list = List.init 3 (fun index->index + 1) 

        printfn "*************** print using 'iter'"
        list |> List.iter (fun item-> printfn "value=%d" item)

        printfn "*************** print using 'iteri'"
        list |> List.iteri (fun index item -> printfn "<%d>: %d" index item)

    let test_iter2() =
        let names = ["dick";"tom";"mary"]
        let scores = [100;80;90]

        printfn "*************** print using 'iter2'"
        List.iter2 (fun name score->printfn "name=%s,score=%d" name score) names scores

        printfn "*************** print using 'iteri2'"
        List.iteri2 (fun index name score-> printfn "[%d] name:%-5s score:%-5d" index name score) names scores

    let test_main() =
        test_iter()
        test_iter2()

[<TestFixture>]
type TestLists() =
    
    [<Test>]
    member this.TestCreateByRange()= 
        // both the start and end values will be included in the list
        [0..3] |> should equal [0;1;2;3]

        // unless the start and the step won't generate the end
        let alist = [0..2..3] 
        alist |> should equal [0;2]
        alist.Length |> should equal 2

        [3..-2..0] |> should equal [3;1]

    [<Test>]
    member this.TestCreateByGenerators()=
        [for index in 1..3 -> index * index] |> should equal [1;4;9]
        [for index in 1..2..4 -> index * index ] |> should equal [1;9]

        [yield 1;
        yield! [100;200]] |> should equal [1;100;200]

        [for index in 1..3 do 
                            let square =  index * index
                            if square%2 = 1 then
                                yield (sprintf "value=%d" square)]
                                |> should equal ["value=1";"value=9"]
        [for a in 1..2 do
            for b in 3..4 do
                yield (a,b)] |> should equal [(1,3);(1,4);(2,3);(2,4);]

    [<Test>]
    member this.TestYieldBang() =
        [for index in 1..2 do yield! [index..index+2]]
            |> should equal [1;2;3;2;3;4]

    [<Test>]
    member this.TestConcat() =
        // ------------------------ append at the header
        1::2::3::[] |> should equal [1;2;3;]

        // ------------------------ append at the tail
        [1;2] @ [3] @[4;5] |> should equal [1;2;3;4;5]

    [<Test>]
    member this.TestCreateByRecursion()=
        let rec array2list (start:int) (array: 't array)  =
            if start = array.Length  then []
            else array.[start] :: (array2list (start + 1) array )

        let array = Array.init 3 (fun index->index + 1) 
        (array2list 0 array) |> should equal [1;2;3]
        (array2list 1 array) |> should equal [2;3]

        array 
            |> Array.map (fun item->string item)
            |> array2list 0
            |> should equal ["1";"2";"3"]

    [<Test>]
    member this.TestConvertWithArray() =
        let array = Array.init 3 (fun index->index + 1)
        let list = Array.toList array
        
        list |> should equal [1;2;3]
        list |> List.toArray |> should equal array

    [<Test>]
    member this.TestInit()=
        List.init 3 (fun index->index * index) 
            |> should equal [0;1;4]

        let prefix = "wsu"
        List.init 2 (fun index -> prefix + string index )
            |> should equal ["wsu0";"wsu1"]

    [<Test>]
    member this.TestLengthAndEmpty()=
        let nonEmpty = [0..3]
        nonEmpty.IsEmpty |> should be False
        // contain both start and end
        nonEmpty.Length |> should equal 4

        let empty = []
        empty.Length |> should equal 0
        empty.IsEmpty |> should be True

    [<Test>]
    member this.TestContain() =
        let list = [0..3]
        // ------------------------- contain any
        list
            |> List.exists (fun item->item%2=0)
            |> should be True

        // ------------------------- contain all
        list
            |> List.forall (fun item -> item > 2)
            |> should be False

    [<Test>]
    member this.TestAccessElement() =
        let list = List.init 3 (fun index->
                                            let value = index + 1
                                            value * value)
        list |> List.head |> should equal 1
        list |> List.tail |> should equal [4;9]

        // chekanote: less effective, NOT best practice
        (List.nth list 1) |> should equal 4

    [<Test>]
    member this.TestFind() =
        List.init 3 (fun index->(index + 1) * (index + 1))
            |> List.find (fun item-> item%2=0)
            |> should equal 4

    [<Test>]
    member this.TestFilter()=
        ["stasi";"kgb";"cheka";"mss";"ss"]
            |> List.filter (fun item->item.StartsWith("s"))
            |> should equal ["stasi";"ss"]

    [<Test>]
    member this.TestSort()=
        // ----------------------- simple sort with natural order: small-->large
        [3;44;7;88;-9] |> List.sort |> should equal [-9;3;7;44;88]
        ["stasi";"kgb";"cheka"] |> List.sort |> should equal ["cheka";"kgb";"stasi"]

        // ----------------------- sort with
        let records = [("tom",100);("dick",88);("mary",66)]
        let sortedByScore = [("mary",66);("dick",88);("tom",100);]
        let sortedByName = [("dick",88);("mary",66);("tom",100);]

        records |> List.sortWith (fun x y -> snd(x) - snd(y))
                |> should equal sortedByScore

        records |> List.sortWith (fun x y -> 
                                            let nameX = fst x
                                            let nameY = fst y
                                            nameX.CompareTo(nameY))
                |> should equal sortedByName

        // ----------------------- sort by
        records |> List.sortBy snd
                |> should equal sortedByScore

        records |> List.sortBy fst
                |> should equal sortedByName
            
    [<Test>]
    member this.TestMap() =
        List.init 3 (fun index -> index + 1)
            |> List.map (fun item-> string item)
            |> should equal ["1";"2";"3"]

        [(1,"tom");(2,"mary")]
            |> List.map (fun (id,name)->sprintf "%d-%s" id name)
            |> should equal ["1-tom";"2-mary"]

    [<Test>]
    member this.TestFold()=
        let sb = List.init 3 (fun index->index + 1)
                    |> List.fold (fun (acc:System.Text.StringBuilder) (item : int)->acc.Append(item)) (new System.Text.StringBuilder())
        sb.ToString() |> should equal "123"

    /// just equivalent to "SelectMany" in C#
    [<Test>]
    member this.TestCollect()=
        ["wsu";"f#"]
            |> List.collect (fun s -> s.ToCharArray() |> List.ofArray)
            |> should equal ['w';'s';'u';'f';'#']

    [<Test>]
    member this.TestRecMatch() =
        let rec count_length (alist: 't list) =
            match alist with
                |[] -> 0
                |[_] -> 1
                |[_;_] -> 2
                |_::tail -> 1 + count_length tail

        [1..2..10] |> count_length |> should equal 5
        ["hello";"wsu";"f#"] |> count_length |> should equal 3

        let rec sum (alist: int list) =
            match alist with 
                |[] -> 0
                |header::tail -> header + sum tail
        [4;5;77;3;55] |> sum |> should equal 144

    [<Test>]
    member this.TestPartition()=
        let evens, odds = [1..10] |> List.partition (fun item->item%2=0)
        evens |> should equal [2;4;6;8;10]
        odds |> should equal [1;3;5;7;9]

    /// because List is implemented by DU, so it automatically has "structual equality"
    [<Test>]
    member this.TestEqual()=
        let list1 = [1;2]
        let list2 = [1;2]

        obj.ReferenceEquals(list1,list2) |> should be False
        (list1 = list2) |> should be True
        list1.Equals(list2) |> should be True
        





        
