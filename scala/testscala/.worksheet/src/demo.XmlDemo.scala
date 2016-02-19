package demo

import xml._

object XmlDemo {;import org.scalaide.worksheet.runtime.library.WorksheetSupport._; def main(args: Array[String])=$execute{;$skip(80); 
  println("hello stasi in Scala");$skip(70); 

  val elem = <a href="http://scala-lang.org">The Scala language</a>;System.out.println("""elem  : scala.xml.Elem = """ + $show(elem ));$skip(12); val res$0 = 
  elem.text;System.out.println("""res0: String = """ + $show(res$0));$skip(26); val res$1 = 
  elem.attributes("href");System.out.println("""res1: Seq[scala.xml.Node] = """ + $show(res$1));$skip(84); 

  val list = <dl><dt>Java</dt><dd>Gosling</dd><dt>Scala</dt><dd>Odersky</dd></dl>;System.out.println("""list  : scala.xml.Elem = """ + $show(list ));$skip(30); 
  val languages = list \ "dt";System.out.println("""languages  : scala.xml.NodeSeq = """ + $show(languages ));$skip(17); val res$2 = 
  languages.text;System.out.println("""res2: String = """ + $show(res$2))}
}
