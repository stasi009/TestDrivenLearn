namespace TestAdvDotNet

open System
open NUnit.Framework
open FsUnit

module Module4Equatable = 
    type RefEqualObj(m_id) = 
        member this.Id = m_id
    
    type OverrideObjEqual(m_id) = 
        member this.Id = m_id
        
        override this.Equals other = 
            if other = null then false
            elif obj.ReferenceEquals(this, other) then true
            elif other :? OverrideObjEqual then m_id = (other :?> OverrideObjEqual).Id
            else false
        
        override this.GetHashCode() = m_id
    
    [<AllowNullLiteral>]
    type Person(m_firstName : string, m_lastName : string) = 
        member this.FirstName = m_firstName
        member this.LastName = m_lastName
        override this.ToString() = sprintf "%s %s" m_firstName m_lastName
        
        override this.Equals other = 
            if other = null then false
            elif obj.ReferenceEquals(this, other) then true
            elif other :? Person then (this :> IEquatable<Person>).Equals(other :?> Person)
            else false
        
        override this.GetHashCode() = this.ToString().GetHashCode()
        interface IEquatable<Person> with
            member this.Equals other = 
                if other = null then false
                elif obj.ReferenceEquals(this, other) then true
                else 
                    (m_firstName.Equals(other.FirstName, StringComparison.OrdinalIgnoreCase)) 
                    && (m_lastName.Equals(other.LastName, StringComparison.OrdinalIgnoreCase))

open Module4Equatable

// note: from this test, we know that in F#, "=" is based on "object.Equals"
// if the class override "object.Equals", then it will automatically overrides "="
[<TestFixture>]
type TestEquatable() = 
    
    [<Test>]
    member this.TestSameType() = 
        let p1 = new Person("a", "b")
        let p2 = new Person("a", "c")
        let p3 = new Person("A", "B")
        p1 |> should not' (equal p2)
        p1 |> should equal p3
        p1 = p3 |> should be True
        p1 |> should equal p1
    
    [<Test>]
    member this.TestDifferentType() = 
        let p1 = new Person("a", "b")
        p1 |> should not' (equal "string")
        let o = new Person("A", "B") :> obj
        p1 |> should equal o
    
    [<Test>]
    member this.TestEqualOperator() = 
        let p1 = new Person("a", "b")
        let p2 = new Person("A", "B")
        let p3 = new Person("c", "d")
        p1 |> should equal p2
        (p1 = p2) |> should be True
        obj.ReferenceEquals(p1, p2) |> should be False
        (p1 = p3) |> should be False
        (p1 <> p3) |> should be True // "<>" is automatically overriden
    
    [<Test>]
    member this.TestReferenceEqual() = 
        let a = new RefEqualObj(1)
        let b = new RefEqualObj(1)
        let copy = a
        a.Id |> should equal b.Id
        a |> should not' (equal b)
        (a = b) |> should be False
        a |> should equal copy
    
    [<Test>]
    member this.TestJustOverrideObjEquals() = 
        let a = new OverrideObjEqual(1)
        let b = new OverrideObjEqual(2)
        let c = new OverrideObjEqual(1)
        a |> should not' (equal b)
        // "=" here in the F# is based on Equals
        a |> should equal c
        (a = c) |> should be True
        obj.ReferenceEquals(a, c) |> should be False
    
    [<Test>]
    member this.TestStringEqual() = 
        let s1 = 
            "stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi "
        let s2 = 
            "stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi stasi "
        let s3 = new String(s1.ToCharArray())
        // I think .NET must do some cache underneatch
        // so static-defined string will only have one copy
        obj.ReferenceEquals(s1, s2) |> should be True
        obj.ReferenceEquals(s1, s3) |> should be False
        (s1 = s2) |> should be True
        (s1 = s3) |> should be True // because String overrides "Equals"
