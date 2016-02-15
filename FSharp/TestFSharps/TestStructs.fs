
namespace TestFSharps

open NUnit.Framework
open FsUnit

[<Struct>]
type Book =
    val Author : string
    val Title : string
    val mutable Price: float

    new(author,title,price) = {Author = author; Title=title;Price = price}
    // in custom constructor, you have to fill all the fields, otherwise there will be a compile error
    new(author,title) = {Author = author; Title=title;Price = 5.0}

[<TestFixture>]
type TestStructs()=

    [<Test>]
    member this.TestSample()=
        let immutableBook = new Book("tom","story",8.1)
        immutableBook.Title |> should equal "story"

        let mutable mutableBook = new Book("mary","history",2.5)
        mutableBook.Price <- mutableBook.Price + 1.1
        mutableBook.Price |> should (equalWithin 1e-6) 3.6

    [<Test>]
    member this.TestCopyWhenAssign()=
        let book1 = new Book("tom","story",8.1)
        // a copy is made when doing assignment, so changes in one won't affect the other
        let mutable book2 = book1

        book2 |> should not' (be sameAs book1)

        book2.Price <- 10.9
        book1.Price |> should (equalWithin 1e-6) 8.1

    [<Test>]
    member this.TestStructuralEquality() =
        let book1 = new Book("tom","novel")
        let book2 = new Book("tom","novel")

        book1 |> should equal book2
        (book1 = book2) |> should be True