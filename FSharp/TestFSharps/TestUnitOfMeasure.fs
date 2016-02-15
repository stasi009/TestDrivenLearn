
namespace TestFSharps

open NUnit.Framework
open FsUnit

module Module4Units =
    [<Measure>]
    type m // meter

    [<Measure>]
    type s // second

    /// composite units
    [<Measure>]
    type kg

    [<Measure>]
    type N = (kg * m) / (s^2)

    [<Measure>]
    type Pa = N / (m^2) 

    /// conversion between types
    [<Measure>]
    type cm

    [<Measure>]
    type inch

    let cmPerInch = 2.54<cm/inch>

    [<Measure>]
    type km

    [<Measure>]
    type mile

    [<Measure>]
    type h

    let mph2Kmph (speed : float<mile/h>)=
        speed * 1.6<km/mile>
open Module4Units

[<TestFixture>]
type TestUnitOfMeasure() =
    
    /// chekanote: below test demonstrate that 
    /// even decorated with Units of Measurement
    /// when checking equality, it still check the value
    /// and ignore the Units
    /// !!! maybe this is because Units are Compile-Time concepts only
    /// !!! but Equal are Runtime-Concepts. Units play no role during runtime
    [<Test>]
    member this.TestEqual() =
        let distance = 100<m>
        let tmcost = 5<s>
        let speed = distance / tmcost
        
        let expected = 20<m/s>
        let unexpected = 20<s/m>

        (speed.Equals expected) |> should be True
        (speed.Equals unexpected) |> should be True

        (speed.Equals 10<m/s>) |> should be False
        // we can see here, when checking equality, a runtime operation
        // it totally get ride of unit, and only compares value inside
        (speed.Equals 20) |> should be True

    /// below codes shows that the same Units can be applied on 
    /// various types
    [<Test>]
    member this.TestMultipleNumTypes()=
        let intDistance = 5<m>
        let floatDistance = 100.0<m>
        ()

    [<Test>]
    member this.TestConversion()=
        let heightInch = 10.0<inch>
        let heightCm = heightInch *  cmPerInch
        
        // chekanote: convert value with Units to value without unit
        let onlyvalue = float heightCm
        onlyvalue |> should (equalWithin 1e-3) 25.4

    [<Test>]
    member this.TestConversion2()=
        let maxSpeed = 50.0<km/h>
        let speed = 40.0<mile/h>

        let flag =
            if (mph2Kmph speed) > maxSpeed then "speeding"
            else "not speeding"

        flag |> should equal "speeding"

