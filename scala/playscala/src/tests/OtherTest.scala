package tests

import org.scalatest.Spec

class OtherTest extends Spec {

  def `define multiple val/var in one line` = {
    val a, b = 9
    assertResult(9)(a)
    assertResult(9)(b)
  }

  def `conditional expression` = {

    def __fool(flag: Boolean) = {
      if (flag) "true" else "false"
    }

    assertResult("true")(__fool(true))
    assertResult("false")(__fool(false))
  }

  def `block expression` = {
    val a = 9
    val b = {
      val x = a + 1
      val y = -a
      x * y
    }
    assert(b === -90)
  }

  def `test tuples` = {
    val t = (1, "stasi")

    // use pattern match to decompose
    val (id, name) = t
    assert(id == 1)
    assert(name == "stasi")

    // use ._1 or ._2 to access elements
    assert(t._1 == 1)
    assert(t._2 == "stasi")

    // pairs can transform to Map
    val keys = Array("stasi", "kgb", "gru")
    val values = Array(1, 9, 8)
    val map = keys.zip(values).toMap

    for (index <- 0 until keys.length) {
      assert(map(keys(index)) == values(index))
    } //for
  }

}