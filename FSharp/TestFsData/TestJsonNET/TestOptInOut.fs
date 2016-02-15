namespace TestJson

open System
open System.Runtime.Serialization
open Newtonsoft.Json

module TestOptInOut = 
    // *************************** classes
    // note: by default, the class's attribute is OptOut
    // which indicates that all public members will be serialized
    // use "JsonIgnore" to ignore certain public member during serialization
    type Obj4Ignore(id, name, value) = 
        let m_id : int = id
        let m_name : string = name
        let m_value : float = value
        member this.Id = m_id
        member this.Name = m_name
        [<JsonIgnore>]
        member this.Value = m_value
    
    // note: 'OptIn' indicates all members will be ignored, none of them will be serialized
    // use attribute 'JsonProperty' to serialize specific member
    [<JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)>]
    type Obj4OptIn(id : int, name : string, value : float) = 
        let m_id = id
        let m_name = name
        let m_value = value
        
        [<JsonProperty>]
        member this.Id = m_id
        
        member this.Name = m_name
        member this.Value = m_value
    
    // similiar as OptIn, only property marked with Attribute "DataMember"
    // will be serialized
    [<DataContract>]
    type Obj4DataContract(id : int, name : string, value : float) = 
        let m_id = id
        let m_name = name
        let m_value = value
        member this.Id = m_id
        
        [<DataMember>]
        member this.Name = m_name
        
        member this.Value = m_value
    
    // *************************** methods
    let private testIgnoreProperty() = 
        new Obj4Ignore(9, "stasi", 88.66)
        |> JsonConvert.SerializeObject
        |> printfn "%s"
    
    let private testOptIn() = 
        new Obj4OptIn(6, "cheka", 9.698)
        |> JsonConvert.SerializeObject
        |> printfn "%s"
    
    let private testDataContract() = 
        new Obj4DataContract(66, "KGB", 9.698)
        |> JsonConvert.SerializeObject
        |> printfn "%s"
    
    let main() = 
        // testIgnoreProperty()
        // testOptIn()
        testDataContract()
