
namespace TestFSharps

module TestMutableModule =
    let mutable private MutableVar = 99
    let Constant = "stasi"

    printfn "!!!!!!!!!!! welcome to TestMutableModule !!!!!!!!!!!"

    let fool1() =
        printfn "fool1"

    let getMutableVar () = MutableVar

    let fool2 number =
        printfn "fool2 with %d" number

module TestConstantModule =
    printfn "############ welcome to TestConstantModule ############"

