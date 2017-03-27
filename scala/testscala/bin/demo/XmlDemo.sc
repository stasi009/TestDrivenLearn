package demo

import xml._

object XmlDemo {
  println("hello stasi in Scala")                 //> hello stasi in Scala

  val elem = <a href="http://scala-lang.org">The Scala language</a>
                                                  //> elem  : scala.xml.Elem = <a href="http://scala-lang.org">The Scala language<
                                                  //| /a>
  elem.text                                       //> res0: String = The Scala language
  elem.attributes("href")                         //> res1: Seq[scala.xml.Node] = http://scala-lang.org

  val list = <dl><dt>Java</dt><dd>Gosling</dd><dt>Scala</dt><dd>Odersky</dd></dl>
                                                  //> list  : scala.xml.Elem = <dl><dt>Java</dt><dd>Gosling</dd><dt>Scala</dt><dd>
                                                  //| Odersky</dd></dl>
  val languages = list \ "dt"                     //> languages  : scala.xml.NodeSeq = NodeSeq(<dt>Java</dt>, <dt>Scala</dt>)
  languages.text                                  //> res2: String = JavaScala
}