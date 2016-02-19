package test

import org.scalatest.Spec

class OtherTest extends Spec {
  
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
      val x = a+1
      val y = -a
      x*y
    }
    assert(b === -90)
  }
  
  def `test tuples` = {
    val t = (1,"stasi")
    
    // use pattern match to decompose
    val (id,name) = t
    assert(id == 1)
    assert(name == "stasi")
    
    // use ._1 or ._2 to access elements
    assert(t._1 == 1)
    assert(t._2 == "stasi")
  }

}