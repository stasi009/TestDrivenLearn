
namespace TestFSharps.Classes

open NUnit.Framework
open FsUnit

type Invoke =
    | Undefined = 0
    | ByParent = 1
    | ByChild = 2

type Parameter()= 
    class
        let mutable m_flag = Invoke.Undefined

        member this.InvokeBy 
            with get() = m_flag
            and set newvalue = m_flag <- newvalue
    end

type Parent(m_name : string)= 
    class
        member this.Name with get()= m_name

        abstract member Run : Parameter->unit
        default this.Run (arg: Parameter)=
            arg.InvokeBy <- Invoke.ByParent

        override this.ToString()= m_name
    end

type Child(name:string, m_id : int)=
    inherit Parent(name)

    member this.Id with get() = m_id

    override this.Run (arg: Parameter)=
        arg.InvokeBy <- Invoke.ByChild

    override this.ToString() = sprintf "%s<%d>" base.Name m_id

type IFool= 
    interface
        // demonstrate how to declare property (both readonly and writable) in the interface
        abstract Name : string 
            with get
        abstract Id : int 
            with get,set

        abstract GetInt : unit->int
        abstract GetMessage : unit->string
    end

type ConstantFool(m_num: int)=
    class
        let mutable m_id = 0

        interface IFool with
            member this.Name 
                with get() = "4test"

            member this.Id 
                with get() = m_id
                and set newvalue = m_id <- newvalue


            member this.GetInt() = m_num
            member this.GetMessage() = string m_num
    end

type GenericFool<'t>(m_values: 't seq)=
    class
        interface IFool with
            member this.Name 
                with get() = "4test"

            member this.Id 
                with get() = -1
                and set newvalue = 
                    raise <| System.NotImplementedException()

            member this.GetInt() = m_values |> Seq.length
            member this.GetMessage() =
                let sb = m_values |> Seq.fold (fun (sb: System.Text.StringBuilder) e -> sb.Append(e) ) (new System.Text.StringBuilder())
                sb.ToString()
    end

[<AbstractClass>]
type TextSink()=
    abstract member WriteString: string->unit
    default this.WriteString msg =
        printfn "%s" msg

module Module4AdvancedClasses = 
    // even "global" functions defined in module can have "auto-upcast" features
    let internal get_invokeflag (x:Parent) : Invoke=
        let p = new Parameter()
        x.Run(p)
        p.InvokeBy

    type internal System.String with
        member this.ToBool()=
            match this.ToLower() with
                |"y"|"1"|"yes" -> true
                |"n"|"0"|"no" -> false
                |_ -> failwith "unrecognized"

    type private Resulter(?input : int)=
        let m_result = 
            let num = defaultArg input 1
            num * 2

        member this.Result = m_result

    type Fool() =
        member this.GetResult(?input:int)=
            // note: must follow following pattern to pass a default-value-argument
            // into another default-value-arguement
            let x = new Resulter(?input = input)
            x.Result

open Module4AdvancedClasses      
       
[<TestFixture>]
type TestAdvancedClasses()=
    [<Test>]
    member this.TestSimple()=
        let f = new Parent("doc")
        (string f) |> should equal "doc"

        let ef = new Child("plan",99)
        (string ef) |> should equal "plan<99>"

    /// note: from this test, we can see that a name bound to "child" can directly call method from base
    /// but a name bound to implementation of interface must first cast to interface type
    /// only after that it can call method from that interface
    [<Test>]
    member this.TestCallbaseFuncs()=
        let child = new Child("dick",101)
        child.Name |> should equal "dick"

        let fool = new ConstantFool(9) 
        (fool :> IFool).GetInt() |> should equal 9
        (fool :> IFool).GetMessage() |> should equal "9"

    [<Test>]
    member this.TestOverride()=
        let p = new Parameter()
        p.InvokeBy |> should equal Invoke.Undefined

        let mutable basePointer = new Parent("")
        basePointer.Run(p)
        p.InvokeBy |> should equal Invoke.ByParent

        basePointer <- new Child("",1)
        basePointer.Run(p)
        p.InvokeBy |> should equal Invoke.ByChild

    [<Test>]
    member this.TestAbstractClass()=
        let textsink_factory (writer : string->unit)=
            {
                // inherit abstract class in object expression, there is no need to use "default"
                // or "override" to decorate the method
                new TextSink() with 
                    member this.WriteString msg = writer msg
            }

        let strsink_factory (sb:System.Text.StringBuilder) =
            textsink_factory (fun msg->sb.Append msg |> ignore)

        let sb = new System.Text.StringBuilder()
        let sink = strsink_factory sb

        ["hello ";"f#";" from wsu"] |> List.iter sink.WriteString
        sb.ToString() |> should equal "hello f# from wsu"


    /// when a function need a base type argument, and you want to pass in derived-type argument
    /// the upcast is automatic and implicit, you don't need to upcast explicitly
    [<Test>]
    member this.TestAutomaticUpcast()=
        new Parent("")|> Module4AdvancedClasses.get_invokeflag |> should equal Invoke.ByParent
        new Child("",12)|> Module4AdvancedClasses.get_invokeflag  |> should equal Invoke.ByChild

        let fool2 (x:IFool) : string =
            sprintf "%d-%s" (x.GetInt()) (x.GetMessage())
        (fool2 (new ConstantFool(921))) |> should equal "921-921"
        (fool2 (new GenericFool<string>(["hello";"f#"]))) |> should equal "2-hellof#"

    [<Test>]
    member this.TestValidDowncast() =
        let parentPointer : Parent = new Child("x",2) :> Parent

        let param1 = new Parameter()
        parentPointer.Run(param1)
        param1.InvokeBy |> should equal Invoke.ByChild

        let childPointer = parentPointer :?> Child
        let param2 = new Parameter()
        childPointer.Run(param2)
        param2.InvokeBy |> should equal Invoke.ByChild

    [<Test>]
    member this.TestInvalidDowncast()=
        let parent = new Parent("")
        (fun()-> parent :?> Child |> ignore) |> should throw typeof<System.InvalidCastException>
        ()// also can represent "returning void, or unit"

    /// like "is" operator in C#
    [<Test>]
    member this.TestCheckCastable()=
        let actualChild = new Child("x",2) :> Parent
        actualChild :? Child |> should be True

        let justParent = new Parent("y")
        justParent :? Child |> should be False

    [<Test>]
    member this.TestInterface()=
        let mutable p : IFool = (new ConstantFool(99) :> IFool)
        p.GetInt() |> should equal 99
        p.GetMessage() |> should equal "99"

        p <- (new GenericFool<string>(["hello";"wsu"]) :> IFool)
        p.GetInt() |> should equal 2
        p.GetMessage() |> should equal "hellowsu"

        p <- (new GenericFool<int>([|100;2;3|]) :> IFool)
        p.GetInt() |> should equal 3
        p.GetMessage() |> should equal "10023"

    [<Test>]
    member this.TestInterfaceAsArgument()=
        // we can see here, we can just simple use interface or abstract classes as the type of the argument
        // chekanote: there is no need to use "#" to force flexible types here
        let fool (arg: IFool) = 
            sprintf "%d-%s" (arg.GetInt()) (arg.GetMessage())

        new ConstantFool(100) |> fool |> should equal "100-100"
        new GenericFool<int>([101;22]) |> fool |> should equal "2-10122" 

    [<Test>]
    member this.TestInterfaceCollectionAsArgs()=
        // also, there is no need to use "#" to force flexible types here
        let fool (arg: IFool seq) =
            arg |> Seq.map (fun item -> sprintf "%d-%s" (item.GetInt()) (item.GetMessage()))

        [(new ConstantFool(101) :> IFool);
        (new GenericFool<int>([88;66]) :> IFool)]
            |> fool
            |> should equal ["101-101";"2-8866"]

    [<Test>]
    member this.TestWithPatternMatching()=
        // !!! chekanote: pay attention that we must match the child type first before the base type
        // !!! otherwise, the child class will never be matched
        let get_description (x:obj) : string=
            match x with 
                | :? Child as c -> sprintf "Child(%s,%d)" c.Name c.Id
                | :? Parent as p -> sprintf "Parent(%s)" p.Name
                | _ -> "Neither"

        new Parent("tom") |> get_description |> should equal "Parent(tom)"
        new Child("mary",101) |> get_description |> should equal "Child(mary,101)"
        "x" |> get_description |> should equal "Neither"

    [<Test>]
    member this.TestExtensionMethods()=
        ["yes";"0";"no";"Y"]
            |> List.map (fun item->item.ToBool())
            |> should equal [true;false;false;true]

    [<Test>]
    member this.TestObjectExpression()=
        let array = [|
            new Child("a",100);
            new Child("b",10)
        |]

        let sorterByName = 
            {
                new System.Collections.Generic.IComparer<Child> with 
                    member this.Compare(x,y) = 
                        x.Name.CompareTo(y.Name)
            }
        System.Array.Sort(array,sorterByName)
        array |> Array.map (fun item->(item.Name,item.Id)) |> should equal [("a",100);("b",10)]

        let sorterById =
            {
                new System.Collections.Generic.IComparer<Child> with
                    member this.Compare(x,y) =
                        x.Id - y.Id
            }
        System.Array.Sort(array,sorterById)
        array |> Array.map (fun item->(item.Id,item.Name)) |> should equal [(10,"b");(100,"a")]

    [<Test>]
    member this.TestPassInOptionalParameter() =
        let x = new Module4AdvancedClasses.Fool()
        x.GetResult() |> should equal 2
        x.GetResult(3) |> should equal 6

