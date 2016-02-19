package test

import org.scalatest.Spec

sealed class OperatorTest extends Spec {

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
  }

  object Fraction {
    def apply(n: Int, d: Int) = new Fraction(n, d)
    def unapply(f: Fraction) = {
      if (f._den == 0) None
      else Some(f._num, f._den)
    }
  }

  def `test operator 1` = {
    val f1 = Fraction(3, 4)
    val f2 = Fraction(2, 5)
    assert(f1 * f2 == Fraction(3, 10))
  }
  
  def `test extractor` = {
    val Fraction(num,den) = Fraction(3, 4) * Fraction(2, 5)
    assert(num == 6)
    assert(den == 20)
  }

}