
namespace TestFSharps

open System
open System.Reflection 
open NUnit.Framework
open FsUnit

module ReflectionExt =
    let (?) (obj:obj) (nm:string) : 'T =  
        obj.GetType().InvokeMember(nm, BindingFlags.NonPublic ||| BindingFlags.Instance |||BindingFlags.GetField, null, obj, [| |])  
        |> unbox<'T> 
open ReflectionExt

module Module4Reflection =
    type Record(id) = 
        let m_usedField = string id
        let m_nonUsedField = ""
        
        member this.Fool = m_usedField.Length 

open Module4Reflection

[<TestFixture>]
type TestReflection() =

    /// this test demonstrate that only field used by that class
    /// will be compiled into the assembly
    /// fields declared but never used will be neglected by the compiler
    /// and never be compiled into assembly, so it can never be reflected
    [<Test>]
    member this.TestDynamicGet() =
        let record = new Record(88)
        
        let usedField : string = record ? m_usedField
        usedField |> should equal "88"

        (fun() -> (record ? m_nonUsedField) |> ignore) 
            |> should throw typeof<MissingFieldException>
        

    

