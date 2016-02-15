
namespace TestFSharps

open NUnit.Framework
open FsUnit

type RefEqualObj(m_num : int)=
    class
    end

/// chekanote: it seems that we cannot add [<CustomEquality;CustomComparison>] to class
/// and it seems ignoring those attributes has no harm, custom equality and custom comparison still works
type ValEqualObj(m_num : int)=
    
    member this.Num = m_num

    override this.Equals(other: obj)=
        match other with
            | :? ValEqualObj as y -> y.Num = m_num
            | _ -> false

    override this.GetHashCode() =
        hash m_num

    interface System.IComparable<ValEqualObj> with
        member this.CompareTo (right: ValEqualObj)=
            compare m_num right.Num

    interface System.IComparable with
        member this.CompareTo (right : obj) =
            match right with
                | :? ValEqualObj as y -> (this :> System.IComparable<ValEqualObj>).CompareTo y
                | _ -> invalidArg "right" "not 'ValEqualObj' type"

[<TestFixture>]
type TestEqualCompare()=

    /// by default, the base class, Object, check "reference equality"
    [<Test>]
    member this.TestDefault_RefEqual()=
        let instance1 = new RefEqualObj(1)
        let instance2 = new RefEqualObj(1)

        (instance1 = instance1) |> should be True
        (instance1 = instance2) |> should be False

    [<Test>]
    member this.TestCustomEqual()=
        let obj1 = new ValEqualObj(1)
        let obj2 = new ValEqualObj(1)
        let obj3 = new ValEqualObj(9)

        obj.ReferenceEquals(obj1,obj2) |> should be False
        (obj1 = obj2) |> should be True
        (obj1 <> obj3) |> should be True

    [<Test>]
    member this.TestCustomCompare()=
        let obj1 = new ValEqualObj(1)
        let obj2 = new ValEqualObj(2)

        (obj1 < obj2) |> should be True
        (obj1 > obj2) |> should be False
