namespace TestFSharps

open System.Collections.Generic
open NUnit.Framework
open FsUnit

module Module4Seq = 
    type ArrayContainer(start, finish) = 
        let m_array = [| start..finish |]
        member this.ReadonlySeq = Seq.readonly m_array
        member this.WritableSeq = m_array :> int seq
    
    let private checkUseBinding() = 
        let sequence = 
            seq { 
                use disposable = 
                    { new System.IDisposable with
                          member this.Dispose() = printfn "!!!!!! DISPOSED" }
                // use
                for index = 1 to 6 do
                    printfn "%d is yield." index
                    yield index
            }
        // seq
        sequence
        |> Seq.take 1
        |> Seq.iter (fun item -> ())
    
    let private checkManualDispose() = 
        let disposable = 
            { new System.IDisposable with
                  member this.Dispose() = printfn "!!!!!! DISPOSED" }
        
        let total = 6
        
        let sequence = 
            seq { 
                for index = 1 to total do
                    printfn "%d is yield." index
                    yield index
                disposable.Dispose()
            }
        //seq
        // note: disposable will be disposed
        sequence |> Seq.iter (fun index -> ())
        // note: by "take", even consumed all, disposable will NOT be disposed
        sequence
        |> Seq.take total
        |> Seq.iter (fun index -> ())
    
    let test_main() = 
        // checkUseBinding()
        checkManualDispose()

[<TestFixture>]
type TestSequences() = 
    
    [<Test>]
    member this.TestSimpleCreate() = 
        { 1..3 } |> should equal [ 1..3 ]
        { 1..2..4 } |> should equal [| 1..2..4 |]
    
    [<Test>]
    member this.TestUnFold() = 
        let numbers = 
            0 |> Seq.unfold (fun state -> 
                     if (state < 3) then Some(string state, state + 1)
                     else None)
        numbers |> should equal [ "0"; "1"; "2" ]
    
    [<Test>]
    member this.TestConvert() = 
        // !!!!!!!!!!!! although sequence is IEnumerable, which includes array and list
        // !!!!!!!!!!!! but since F# doesn't support auto-cast
        // !!!!!!!!!!!! so direct assignment like: let intSeq : int seq = [1;2] 
        // !!!!!!!!!!!! will cause compile error
        let intarray = Array.init 3 (fun index -> index + 1)
        let intSeq : int seq = intarray |> Array.toSeq
        intSeq |> should equal intarray
        let strlist = [ "hello"; "wsu" ]
        let strSeq : string seq = strlist |> List.toSeq
        strSeq |> should equal strlist
    
    [<Test>]
    member this.TestCreateByExpression() = 
        // ------------ using "for..do..yield"
        let seq1 = 
            seq { 
                for index in 1..3 do
                    if index % 2 = 0 then // no need to provide else clause
                        let name = "hello " + string index
                        yield (index, name)
            }
        seq1 |> should equal [ (2, "hello 2") ]
        // ------------ using "for..in->"
        let seq2 = 
            seq { 
                for index in 1..2..4 -> (index, index * index)
            }
        seq2 |> should equal [ (1, 1)
                               (3, 9) ]
    
    [<Test>]
    member this.TestYieldBang() = 
        seq { 
            for index in 1..2 do
                yield (sprintf "parent-%d" index)
                yield! (seq { index + 1..index + 2 } |> Seq.map (fun item -> sprintf "child-%d" item))
        }
        |> should equal [ "parent-1"; "child-2"; "child-3"; "parent-2"; "child-3"; "child-4" ]
        // here when using Split, in C#, this method will accept a param array, so at least for one item
        // you can just pass in that single item, without wrapping it into [||]
        seq { 
            for item in [ "hello wsu"; "stasi F#" ] do
                yield! (item.Split ' ')
        }
        |> should equal [ "hello"; "wsu"; "stasi"; "F#" ]
    
    [<Test>]
    member this.TestCreateFromPattern() = 
        let records = 
            [ ("tom", 100)
              ("mary", 80) ]
        seq { 
            for (name, score) in records do
                yield (sprintf "%s's score is %d" name score)
        }
        |> should equal [ "tom's score is 100"; "mary's score is 80" ]
    
    [<Test>]
    member this.TestInfiniteSequence() = 
        let counter = ref 0
        
        // define recursive values
        let rec infinite_numbers = 
            seq { 
                incr counter
                yield !counter
                yield! infinite_numbers
            }
        infinite_numbers
        |> Seq.take 4
        |> should equal [ 1; 2; 3; 4 ]
    
    /// even only part of the original sequence is consumed
    /// its use-binding still works to release the resource
    [<Test>]
    member this.TestUseBinding() = 
        let disposed = ref false
        
        let sequence = 
            seq { 
                use disposable = 
                    { new System.IDisposable with
                          member this.Dispose() = disposed := true }
                //use
                for index = 1 to 5 do
                    yield index
            }
        // seq
        sequence
        |> Seq.take 2
        |> should equal [ 1; 2 ]
        !disposed |> should equal true
    
    /// chekanote: manual dispose is different from using "use binding within seq"
    /// if it cannot reach the end, then the Disposable won't be invoked
    /// so may cause leakage
    [<Test>]
    member this.TestManualDispose() = 
        let fool total = 
            let disposed = ref false
            
            let disposable = 
                { new System.IDisposable with
                      member this.Dispose() = disposed := true }
            
            // disposable
            let sequence = 
                seq { 
                    for index = 1 to total do
                        yield index
                    disposable.Dispose()
                }
            
            disposed, sequence
        
        let donothing index = ()
        // ----------- partial consumed
        let disposed1, sequence1 = fool 5
        sequence1
        |> Seq.take 1
        |> Seq.iter donothing
        !disposed1 |> should equal false
        // ----------- all consumed, BUT with take, STILL NOT DISPOSED
        let disposed2, sequence2 = fool 5
        sequence2
        |> Seq.take 5
        |> Seq.iter donothing
        !disposed2 |> should equal false
        // ----------- all consumed, can be DISPOSED
        let disposed3, sequence3 = fool 6
        sequence3 |> Seq.iter donothing
        !disposed3 |> should equal true
    
    [<Test>]
    member this.TestAppend() = (// array and list can be both viewed as sequence
                                Seq.append [| 1; 2 |] [ 3; 4; 5 ]) |> should equal [ 1; 2; 3; 4; 5 ]
    
    [<Test>]
    member this.TestConcat() = 
        let array = [| 1; 2 |]
        let list = [ 3; 4 ]
        let netlist = new System.Collections.Generic.List<int>([ 6 ])
        let collectionOfCollection : seq<int> list = [ array; list; netlist ]
        collectionOfCollection
        |> Seq.concat
        |> should equal [ 1; 2; 3; 4; 6 ]
    
    [<Test>]
    member this.TestMap() = 
        let array = [| 1; 3 |]
        let list = [ "hello"; "wsu" ]
        (// the arguments are both "seq"-compatible type, but their detailed type can be different
         Seq.map2 (fun item1 item2 -> sprintf "%d-%s" item1 item2) array list) |> should equal [ "1-hello"; "3-wsu" ]
    
    /// sequence is just IEnumerable, so functions in Seq module can accept a lot of types as its input
    /// including Array, F# list, .NET List
    /// there is no need to use # (flexible type) here
    [<Test>]
    member this.TestWorkAsGeneralType() = 
        // it will auto-upcast array,list, .NET list to sequence when applying to the parameters
        let add_prefix (prefix : string) (collection : int seq) = 
            collection |> Seq.map (fun item -> prefix + string item)
        List.init 3 (fun index -> index + 1)
        |> add_prefix "hello"
        |> should equal [| "hello1"; "hello2"; "hello3" |]
        Array.init 3 (fun index -> index * index)
        |> add_prefix "wsu"
        |> should equal [ "wsu0"; "wsu1"; "wsu4" ]
        let alist = new List<int>()
        alist.Add(1001)
        alist.Add(88)
        alist
        |> add_prefix "fsharp"
        |> should equal [ "fsharp1001"; "fsharp88" ]
    
    [<Test>]
    member this.TestGroupby() = 
        let dict = 
            [ 1..4 ]
            |> Seq.groupBy (fun item -> item % 2)
            |> Map.ofSeq
        dict.[0] |> should equal [ 2; 4 ]
        dict.[1] |> should equal [ 1; 3 ]
        dict.Count |> should equal 2
    
    [<Test>]
    member this.TestZip() = 
        let seq1 = [ 1; 2 ]
        let seq2 = [ "hello"; "wsu"; "f#" ]
        // the length of the result is determined by the shorter one of the input
        Seq.zip seq1 seq2 |> should equal [ (1, "hello")
                                            (2, "wsu") ]
    
    [<Test>]
    member this.TestCache() = 
        let counter = ref 0
        
        let numbers = 
            seq { 
                for index = 1 to 3 do
                    incr counter
                    yield index
            }
        // ******************* without cache
        numbers |> Seq.iter ignore
        !counter |> should equal 3
        numbers |> Seq.iter ignore
        !counter |> should equal 6
        // ******************* WITH CACHE
        counter := 0
        let cached_numbers = Seq.cache numbers
        // side effect of underlying sequence occurs only once
        cached_numbers |> Seq.iter ignore
        !counter |> should equal 3
        cached_numbers |> Seq.iter ignore
        !counter |> should equal 3 // following operation won't repeat side effect
    
    /// collect is just like LINQ's "SelectMany"
    [<Test>]
    member this.TestCollect() = 
        [ 1..2 ]
        |> Seq.collect (fun item -> 
               seq { 
                   yield -item
                   yield item
               })
        |> should equal [ -1; 1; -2; 2 ]
    
    [<Test>]
    member this.TestReadonly() = 
        let container = new Module4Seq.ArrayContainer(1, 3)
        container.ReadonlySeq |> should equal [ 1; 2; 3 ]
        let underline = container.WritableSeq :?> int array
        underline.[1] <- 800
        container.ReadonlySeq |> should equal [ 1; 800; 3 ]
        (fun () -> 
        let impossible = container.ReadonlySeq :?> int array
        impossible.[0] <- 66
        ())
        |> should throw typeof<System.InvalidCastException>
    
    [<Test>]
    member this.TestSort() = 
        let records = 
            seq { 
                yield ("tom", 100)
                yield ("dick", 88)
                yield ("mary", 66)
            }
        
        let sortedByScore = 
            [ ("mary", 66)
              ("dick", 88)
              ("tom", 100) ]
        
        let sortedByName = 
            [ ("dick", 88)
              ("mary", 66)
              ("tom", 100) ]
        
        records
        |> Seq.sortBy snd
        |> should equal sortedByScore
        records
        |> Seq.sortBy fst
        |> should equal sortedByName
    
    [<Test>]
    member this.TestMaxMin() = 
        let names = [ "vcheka"; "stasi"; "kgb"; "gestapo" ]
        names
        |> Seq.min
        |> should equal "gestapo"
        names
        |> Seq.max
        |> should equal "vcheka"
        let getlength = fun (s : string) -> s.Length
        names
        |> Seq.minBy getlength
        |> should equal "kgb"
        names
        |> Seq.maxBy getlength
        |> should equal "gestapo"
