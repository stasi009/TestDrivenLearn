package test

import org.scalatest.Spec

sealed class LazyValueTest extends Spec {
  
  object `lazy values` {
    
    def `demo1 evaluated only once` = {
      var a = 1
      lazy val b = a + 1
      
      // lazy values are lazy evaluated
      // so it will use the value of "a" when it is first time accessed
      a = 10
      assert(b == 11)
      
      // lazy value is evaluated only once
      // after that, its value are fixed
      a = 100
      assert(b == 11)
    }
    
    def `demo2 evaluated only once` = {
      var counter = 0
      lazy val x = {
        counter += 1
        100
      }
      
      assert(counter == 0)// not evaluated yet
      
      val r1 = x + 1
      assert(counter == 1)// first time accessed, evaluate once
      assertResult(r1)(101)
      
      val r2 = x - 1
      assertResult(99)(r2)
      assertResult(r2)(99)// evaluate only once
    }
  }

}