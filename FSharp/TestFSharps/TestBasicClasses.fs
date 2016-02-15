namespace TestFSharps.Classes

open System
open NUnit.Framework
open FsUnit

module Module4BasicClasses = 
    type Fool() = 
        // -------------- static constructors
        static let mutable m_objCounter = 0
        
        static do 
            printfn "!!! static constructor runs"
            printfn "!!! (it should run before any instance is created, and should run only once)"
        
        // -------------- instance constructors
        do m_objCounter <- m_objCounter + 1
        let m_id = m_objCounter
        do printfn "\tnew instance is created with id<%d>" m_id
        static member Add (x : int) y = x + y
    
    type Fool2() = 
        
        /// !!!!!!!!!!! chekanote: class can have overloaded methods
        member this.Func(x : int) = string x
        
        member this.Func(x : string) = x
    
    let fool (x : int) = string x
    
    // !!!!!!!!!!!! chekanote: module cannot allow overload functions
    // let fool (x:string) = x
    let test_main() = 
        let mutable input = ""
        while input.Equals("exit") = false do
            new Fool() |> ignore
            printfn "input to continue, ......"
            input <- Console.ReadLine()
        printfn "done !!!"

type Movie(m_title : string, ?director : string, ?ticketPrice : int) = 
    
    // *************************** private member fields ***************************//
    // we can use the same name as that optional argument, because "?name" and "name" are viewed different
    let m_director : string = 
        match director with
        | Some(name) -> name
        | None -> "n/a"
    
    let mutable m_ticketPrice = defaultArg ticketPrice 10
    // *************************** private member methods ***************************//
    let get_description format = sprintf format m_title m_director
    // *************************** overload constructors ***************************//
    // overloaded constructor must call the primary constructor
    new() = Movie("unknown", "unknown")
    // *************************** properties ***************************//
    member this.Title = m_title
    member this.Director = m_director
    
    member this.TicketPrice 
        with get () = m_ticketPrice
        and set (newvalue) = m_ticketPrice <- newvalue
    
    // *************************** methods ***************************//
    // !!! chekanote: arguments of constructor are available in the entire class scope
    // !!! those arguments can be treated as implicit private fields
    override this.ToString() = get_description "title='%s',director='%s'"

// *************** rec class definition *************** //
type Student(name : string, school : University) as this = 
    do this |> school.Enroll
    member this.Name = name

and University() = 
    let m_students = new ResizeArray<Student>()
    member this.Enroll(student : Student) = m_students.Add(student)
    member this.Students = m_students
    member this.Item // override the [] operator
        with get (index : int) = m_students.[index]

// *************** end rec class definition *************** //
[<TestFixture>]
type TestBasicClasses() = 
    
    [<Test>]
    member this.TestSimple() = 
        // pass the arguments by position
        let movie1 = new Movie("story", "tom")
        (string movie1) |> should equal "title='story',director='tom'"
        // pass the arguments by name
        let movie2 = new Movie(director = "mary", m_title = "tale")
        (string movie2) |> should equal "title='tale',director='mary'"
    
    [<Test>]
    member this.TestInitializer() = 
        let movie_defaultPrice = new Movie()
        movie_defaultPrice.TicketPrice |> should equal 10
        let movie_explicitPrice = new Movie(TicketPrice = 111)
        movie_explicitPrice.TicketPrice |> should equal 111
    
    [<Test>]
    member this.TestOverloadConstructor() = 
        let movie = new Movie()
        movie.Title |> should equal "unknown"
        movie.Director |> should equal "unknown"
        movie.TicketPrice |> should equal 10
    
    [<Test>]
    member this.TestOptionalArgs() = 
        let fullInfo = new Movie("story", "tom")
        (string fullInfo) |> should equal "title='story',director='tom'"
        let partInfo = new Movie("tale")
        (string partInfo) |> should equal "title='tale',director='n/a'"
    
    [<Test>]
    member this.TestDefaultValue() = 
        let movie1 = new Movie("story", "tom", 5)
        movie1.TicketPrice |> should equal 5
        let movie2 = new Movie("tale", "mary")
        movie2.TicketPrice |> should equal 10
    
    [<Test>]
    member this.TestReferenceTypeFeature() = 
        let movie = new Movie("story", "tom")
        movie.TicketPrice |> should equal 10
        let alias = movie
        alias |> should be (sameAs movie)
        Assert.AreSame(alias, movie)
        alias.TicketPrice <- 6
        movie.TicketPrice |> should equal 6
    
    [<Test>]
    member this.TestProperties() = 
        let movie = new Movie("tale", "dick")
        // -------------------- get
        movie.Title |> should equal "tale"
        movie.Director |> should equal "dick"
        movie.TicketPrice |> should equal 10
        // -------------------- set
        movie.TicketPrice <- 15
        movie.TicketPrice |> should equal 15
    
    [<Test>]
    member this.TestRecursiveDefineClasses() = 
        let university = new University()
        new Student("tom", university) |> ignore
        new Student("mary", university) |> ignore
        university.Students
        |> Seq.map (fun s -> s.Name)
        |> should equal [ "tom"; "mary" ]
    
    [<Test>]
    member this.TestOverloadMethods() = 
        let x = new Module4BasicClasses.Fool2()
        x.Func 99 |> should equal "99"
        x.Func "hello" |> should equal "hello"
    
    [<Test>]
    member this.TestStaticMethod() = Module4BasicClasses.Fool.Add 1 2 |> should equal 3
    
    [<Test>]
    member this.TestUserdefIndexer() = 
        let university = new University()
        new Student("tom", university) |> ignore
        new Student("andy", university) |> ignore
        university.[1].Name |> should equal "andy"
