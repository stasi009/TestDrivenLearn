package test

import org.scalatest.Spec
import common.Utility

sealed class NumbersTest extends Spec {
  
  object `basic math` {
    
    def `divide as int and float` = {
      val a = 5
      val b = 2
      
      // integer / integer return another integer
      assertResult(2)(a / b)
      
      // double / double return another double
      assert(Utility.numAlmostEqual(2.5, a.toDouble / b.toDouble))
    }
    
    def `% operator` = {
      assertResult(1)(10%3)
      assert(Utility.numAlmostEqual(0.8, 6.8%2))
    }
  }
  
  object `demo api usages` {
    
    def `define numbers` = {
      assert(9.isInstanceOf[Int])
      assert(9L.isInstanceOf[Long])
      assert(9.9.isInstanceOf[Double])
      assert(9.9f.isInstanceOf[Float])
    }
    
    def `convert between number and string` = {
      assertResult("10")(10.toString)
      assertResult(9)("9".toInt)
      
      // whitespace will also make the conversion failure
      intercept[NumberFormatException]{ "        9".toInt }
    }
    
    def `float to int` = {
      // toInt, just truncate all the digits
      assertResult(99)(99.9.toInt)
      
      // ceil and floor, returns double
      assert(Utility.numAlmostEqual(100.0, 99.9.ceil))
      assert(Utility.numAlmostEqual(99.0, 99.9.floor))
      
      // round, returns integers
      assert(Utility.numAlmostEqual(99, 99.1.round))
      assert(Utility.numAlmostEqual(100, 99.6.round))
    }
  }
}

