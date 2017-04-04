package common

import scala.annotation.tailrec

object Utility {

  // !!! we cannot name both methods as "almost equal"
  // !!! overload and "default arguments" cannot happen at the same time
  def seqAlmostEqual(s1: Seq[Double], s2: Seq[Double], tolerance: Double = 1e-6) = {
    (s1 zip s2) map { t => (t._1 - t._2).abs } forall { _ < tolerance }
  }

  def numAlmostEqual(x: Double, y: Double, tolerance: Double = 1e-6) = {
    (x - y).abs < tolerance
  }

  implicit def double2int(d: Double) = d.toInt

  // implicit class not be top-level class
  // and AnyVal cannot be nested inside another class
  // extending AnyVal is to avoid the runtime allocation 
  // (compiler will treat it like extension method in C#, which is defined as static method in static class)
  implicit class MyRangeMaker(val left: Int) extends AnyVal {
    def -->(right: Int) = left to right
  }
}//object