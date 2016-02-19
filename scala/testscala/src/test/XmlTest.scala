package test

import org.scalatest.Spec
import xml._

sealed class XmlTest extends Spec {

  object `basic demo` {

    def `check types` = {
      val doc = <html><head><title>stasi</title></head><body>...</body></html>
      assert(doc.isInstanceOf[Elem])

      // !!! pay attention ";" is required, indispensable
      // !!! otherwise, the compiler will assume that line is not ended
      // !!! so define a "NodeSeq" need the ";" at the end
      val items = <li>Fred</li><li>Wilma</li>;
      assert(items.isInstanceOf[NodeBuffer])
    } //def

    def `access attributes` = {
      val elem = <a href="http://scala-lang.org">The Scala language</a>
      assertResult("The Scala language")(elem.text)

      // ------------- method 1 to get attributes
      val href = elem.attributes("href")
      assert(href.isInstanceOf[Seq[Node]])
      assert(href.text == "http://scala-lang.org")

      // when accessing non-existed key, it return null
      assert(elem.attributes("not existed") == null)

      // ------------- method 2 to get attributes
      val hrefOpt = elem.attribute("href")
      assert(hrefOpt.isInstanceOf[Option[Seq[Node]]])
      assert(hrefOpt.get.text == "http://scala-lang.org")
    } //def

  } //object

  object `test Elem` {

    def `call text to concatenate` = {
      val nodes = <nodes><a>stasi</a><b>kgb</b></nodes>
      assert(nodes.text == "stasikgb")
    } //def
  } //object

  object `test Node sequence` {

    def `demo 1` = {
      val items = <li>Fred</li><li>Wilma</li>;
      val labels = for (nd <- items) yield nd.label
      assert(labels == Seq("li", "li"))
    } //def

    def `build using NodeBuffer` = {
      val items = new NodeBuffer
      items += <a>Fred</a>
      items += <b>Wilma</b>

      // implicit convert to immutable NodeSeq
      val nodes: NodeSeq = items
      assert((nodes map { _.label }) == Seq("a", "b"))
    } //def

    def `call text to concatenate` = {
      val nodes = <a>stasi</a><b>kgb</b>;
      assert(nodes.text == "stasikgb")
    } //def

  } //object

}//class