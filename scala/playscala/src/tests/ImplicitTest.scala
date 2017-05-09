package tests

import org.scalatest.Spec

sealed class ImplicitTest extends Spec {

  sealed class Fraction(n: Int, d: Int) {
    private val _num = n
    private val _den = d
    private val _value = n.toDouble / d.toDouble

    def *(other: Fraction) = new Fraction(_num * other._num, _den * other._den)

    override def equals(other: Any) = {
      if (other.isInstanceOf[Fraction]) {
        val otherfraction = other.asInstanceOf[Fraction]
        (_value - otherfraction._value).abs < 1e-6
      } else false
    }

    override def hashCode = _value.hashCode
  } //class

  object Fraction {
    def apply(n: Int, d: Int) = new Fraction(n, d)

    // companion object will be a good place to host implicit function
    implicit def int2Fraction(n: Int) = new Fraction(n, 1)
  }

  object `implicit conversion` {

    def `test simple double2int` = {
      import common.Utility.double2int
      val n: Int = 8.9
      assert(n == 8)
    } //def

    def `test implicit class` = {
      import common.Utility.MyRangeMaker
      assertResult(1 to 100) { 1 --> 100 }
    }

    def `implicit conversion in companion object` = {
      val product = 2 * Fraction(3, 4)
      assert(product == Fraction(3, 2))
    } //def
  } //object

  object `implicit parameter` {

    case class Delimiter(left: String, right: String)

    // the default parameter when no explicit parameter is provided
    implicit val quoteDelimiter = Delimiter("<", ">")

    def quote(msg: String)(implicit delimiter: Delimiter) = delimiter.left + msg + delimiter.right

    def `use explicit parameter` = {
      assertResult("(hello scala)")(quote("hello scala")(Delimiter("(", ")")))
    } //def

    def `use implicit parameter` = {
      assertResult("<hello scala>")(quote("hello scala"))
    } //def

  } //object

}