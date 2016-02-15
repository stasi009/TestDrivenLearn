
namespace TestFSharps

open NUnit.Framework
open FsUnit

module Module4ControlFlow =
    let exist_callback (collection: int list) wanted (callback : int->string option) : string option =
        if (collection |> List.exists (fun item -> item = wanted)) then callback wanted
        else None

    let exist (collection: int list) wanted : int option=
        if (collection |> List.exists (fun item -> item = wanted)) then Some(wanted)
        else None

    let find3_bycallback (collection: int list) a b c=
        exist_callback collection a (fun find1 ->
            exist_callback collection b ( fun find2 ->
                exist_callback collection c (fun find3 -> Some(sprintf "%d-%d-%d" find1 find2 find3))))

    type Builder() =
        member this.Bind(optionFinding: int option,callback: int->string option)=
            match optionFinding with
                | Some(find) -> callback find
                | None -> None
        member this.Return x = Some(x)

    let find3_byworkflow (collection: int list) a b c =
        let builder = new Builder()
        builder {
            let! find1 = exist collection a
            let! find2 = exist collection b
            let! find3 = exist collection c
            return sprintf "%d.%d.%d" find1 find2 find3
        }

    let find3_workflow_expanded(collection : int list) a b c =
        let builder = new Builder()
        builder.Bind( (exist collection a),(fun find1->
            builder.Bind((exist collection b),(fun find2->
                builder.Bind((exist collection c),(fun find3-> 
                    builder.Return(sprintf "%d*%d*%d" find1 find2 find3)))))))

    let inrange low high value= 
        if value >= low && value <= high then Some(value)
        else None

    let inrange_byworkflow low high a b=
        let builder = new Builder()
        let result = builder{
            let! ok1 = inrange low high a
            let! ok2 = inrange low high b
            return sprintf "<%d,%d> both in range" ok1 ok2
        }
        match result with
            | Some(msg) -> msg
            | None -> "not in range"
            
open Module4ControlFlow

[<TestFixture>]
type TestComputationExpressions()=
    class

    [<Test>]
    member this.TestCallback()=
        (find3_bycallback [1;2;3;4;] 1 2 3).Value |> should equal "1-2-3"
        (find3_bycallback [1;2;3;4;] 1 2 8).IsNone |> should be True

    [<Test>]
    member this.TestWorkflow()=
        (find3_byworkflow [0;100;3;200] 0 200 100).Value |> should equal "0.200.100"
        (find3_byworkflow [0;100;3;200] 0 200 101).IsNone |>  should be True

    [<Test>]
    member this.TestWorkflow2()=
        (inrange_byworkflow 0 100 88 99) |> should equal "<88,99> both in range"
        (inrange_byworkflow 1 10 7 66) |>  should equal "not in range"

    [<Test>]
    member this.TestWorkflowExpanded()=
        (find3_workflow_expanded [1;2;3;4;] 1 2 3).Value |> should equal "1*2*3"
        (find3_workflow_expanded [1;2;3;4;] 1 2 8).IsNone |> should be True

    end