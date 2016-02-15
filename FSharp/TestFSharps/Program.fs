
open System
open NUnit.Framework
open FsUnit

open TestFSharps
open TestFSharps.TestMutableModule

open TestFSharps.Records
open TestFSharps.Classes.Module4AdvancedClasses
open TestFSharps.Async

module MainTest =

    printfn "!!!!!! Module Codes Executed (only for module in last file) !!!!!!"

    let private test_extension_methods()=
        ["yes";"n";"N";"1";"0";"Yes";"no"]
            |> List.iter (fun item->printfn "%-5s: %b" item (item.ToBool()))

    let pause() =
        printfn ">>>>>>> Press ENTER to continue, ......"
        Console.ReadLine() |> ignore

    let private test_execute_module() =
        printfn "statement in module NOT be executed"
        fool1()
        fool2 88

        pause()
        printfn "access constants will NOT execute the statements in module"
        printfn "Constant='%s'" Constant
        
        pause()
        printfn "!!!!!!!!! access mutable will cause the statements in module executed"
        printfn "Mutable=%d" (getMutableVar()) 
        

    let test_main() =
        // test_extension_methods()
        test_execute_module()

// TestPrint.test_main()
// Func4Test.test_lazy()
// Module4TestLists.test_main()
// TestFsUnit.TestMain()
// Classes.Module4BasicClasses.test_main()
// Module4AsyncWorkflow.test_main()
// Module4Mailbox.test_main()
// Module4Events.test_main()
// MainTest.test_main()
Module4Seq.test_main()