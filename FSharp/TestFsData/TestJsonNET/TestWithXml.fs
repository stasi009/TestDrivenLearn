
namespace TestJson

open System
open System.Xml
open Newtonsoft.Json

module TestWithXml =

    let private fromXml2Json() =
        let xml = @"<?xml version=""1.0"" standalone=""no""?>
                        <root>
                          <person id=""1"">
                          <name>Alan</name>
                          <url>http://www.google.com</url>
                          </person>
                          <person id=""2"">
                          <name>Louis</name>
                          <url>http://www.yahoo.com</url>
                          </person>
                        </root>"

        let xmldoc = new XmlDocument()
        xmldoc.LoadXml xml

        let json = JsonConvert.SerializeXmlNode(xmldoc,Formatting.Indented)

        printfn "***************** XML:\n%s" xml
        printfn "***************** JSON:\n%s" json

    let private fromJson2Xml() =
        let json = @"{
                      ""?xml"": {
                        ""@version"": ""1.0"",
                        ""@standalone"": ""no""
                      },
                      ""root"": {
                        ""person"": [
                          {
                            ""@id"": ""1"",
                            ""name"": ""Alan"",
                            ""url"": ""http://www.google.com""
                          },
                          {
                            ""@id"": ""2"",
                            ""name"": ""Louis"",
                            ""url"": ""http://www.yahoo.com""
                          }
                        ]
                      }
                    }"
        let xmldoc = JsonConvert.DeserializeXmlNode json
        printfn "***************** InnerXml:\n%s" <| xmldoc.InnerXml
        printfn "***************** OuterXml:\n%s" <| xmldoc.OuterXml

    let main() =
        // fromXml2Json()
        fromJson2Xml()



