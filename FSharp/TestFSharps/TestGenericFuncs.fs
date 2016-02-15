
namespace TestFSharps

open NUnit.Framework
open FsUnit

module Module4GenericFuncs=
    // functions with explicit type can be only defined for modules or for classes
    let is_bigger<'t when 't :> System.IComparable<'t> > (x:'t)  (y:'t) = x.CompareTo y > 0

    let array_of_list<'t> length : list<'t> [] = 
        Array.create length []

    let swap_explicit<'t,'m> (a:'t,b:'m) = (b,a)

[<TestFixture>]
type TestGenericFuncs() =
    
    [<Test>]
    member this.TestAutoGenerics() =
        // ---------- inference as generic function
        // ---------- auto-generic, because the operation in the body are independent of types
        let first a b = a
        (first "hello" 1) |> should equal "hello"
        (first 1 "hello") |> should equal 1

        // ---------- chekanote: delayed-inference when first-time invoked 
        // ---------- chekanote: F# always attemp to make it generic, unless there is a reason it cannot
        // ---------- here it has a reason that it cannot: because "+" is not generic for all types
        // ---------- XXXXXXXXXXX (add 1 2) |> should equal 3
        let add a b = a + b
        let result = add "hello" "wsu"
        result |> should equal "hellowsu"

    [<Test>]
    member this.TestExplicitArgTypes()=
        let fool f g (x,y) = (f x,g y)

        let specific_fool f g (x,y)=
            fool f g (x,y) : int*string// specify the result type

        let result1 = specific_fool (fun (arg: string)->arg.Length) (fun arg -> string arg) ("hello",10)
        result1 |> should equal (5,"10")

        let result2 = specific_fool (fun (arg: float) -> int arg) (fun arg-> arg) (3.14,"f#")
        result2 |> should equal (3,"f#")

    [<Test>]
    member this.TestExplicitArgTypes2()=
        let array1 = Module4GenericFuncs.array_of_list<int> 2
        array1.[0] <- [1;2]

        let array2 = Module4GenericFuncs.array_of_list<string> 3
        array2.[2] <- ["hello";"f#"]

    [<Test>]
    member this.TestExplicitArgTypes3()=
        let swap (a:'t,b:'m) = (b,a)
        // !!!!!!!!!!!!!! we cannot explicitly specify the argument type, otherwise it will cause compile-error
        // (swap<int,int> (1,2)) |> should equal (2,1)
        (swap ("hello",1)) |> should equal (1,"hello")
        
        // !!!!!!!!!!!!!! we can only explicitly specify the argument type if they are declard explicitly
        // !!!!!!!!!!!!!! but whether explicitly specifying it or not is totally optional
        // !!!!!!!!!!!!!! type-inference is still working
        (Module4GenericFuncs.swap_explicit ("hello",1)) |> should equal (1,"hello")
        (Module4GenericFuncs.swap_explicit<string,int> ("hello",1)) |> should equal (1,"hello")

    [<Test>]
    member this.TestExplicitGeneric() =
        let auto_generic_swap (a,b) = (b,a)
        (auto_generic_swap ("hello",1)) |> should equal (1,"hello")

        let explicit_swap (a:'t,b:'t) = (b,a)
        (explicit_swap (1,2)) |> should equal (2,1)
        (explicit_swap ("hello","wsu")) |> should equal ("wsu","hello")

    [<Test>]
    member this.TestRequirement() = 
        // ------------ here pick_max "requires comparison"
        let pick_max a b =
            if a >= b then a else b

        (pick_max 1 2 ) |> should equal 2
        (pick_max "hello" "wsu") |> should equal "wsu"

        // ----------- type without implementing IComparable
        (*
        let obj1 = new NonComparable()
        let obj2 = new NonComparable()
        // chekanote: it will cause COMPILE-TIME (NOT runtime) error, due to not support IComparable
        pick_max obj1 obj2 |> ignore
        *)

    [<Test>]
    member this.TestInline()=
        let inline add x y =  x + y

        (add 1 2 ) |> should equal 3
        (add 3.3 6.6) |> should (equalWithin 1e-6) 9.9
        (add "hello " "f#") |> should equal "hello f#"

    [<Test>]
    member this.TestTypeConstraints()=
        (Module4GenericFuncs.is_bigger 10 3) |> should be True
        (Module4GenericFuncs.is_bigger "cheka" "stasi") |> should be False

    [<Test>]
    member this.TestUsingBox()=
        // chekanote: because we are calling "ToString", so "x" is inferred as obj
        // other than a generic type
        let fool_obj x = string x

        // this time, because we are using box which can accept any type
        // so this time, x is inferred as generic type
        let fool_generic x = (box x).ToString()

        fool_obj 100 |> should equal "100"
        fool_generic 1 |> should equal "1"

