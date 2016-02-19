package test

import org.scalatest.Spec

sealed class OptionTest extends Spec {

  object `demo api usage` {
    
    def `for loop against Optional type` = {
      val some = Some((1,"stasi"))
      val value = for (t <- some) yield t._1 
      assert(value.isInstanceOf[Option[Int]])
      assert(value.get == 1)
    }//def
    
    def `is defined, is empty` = {
      val some = Some(9)
      val none = None

      assert(some.isDefined)
      assert(!none.isDefined)
      assert(none.isEmpty)
    }

    def `get, getOrElse` = {
      val some = Some(9)
      assertResult(9) { some.get }

      val none = None
      intercept[NoSuchElementException] { none.get }

      assertResult(-1)(none.getOrElse(-1))
    }

    // the argument passed into 'getOrElse' in "call-by-name" fashion
    // it is passed in as a function, which will be invoked only when necessary
    def `getOrElse is call-by-name` = {

      var counter = 0
      def defaultValue() = {
        counter += 1
        counter * counter
      }

      assertResult(101)(Some(101).getOrElse(defaultValue))
      assertResult(0)(counter) // not invoked yet

      assertResult(1)(None.getOrElse(defaultValue))
      assertResult(1)(counter) // invoked once

      assertResult(4)(None.getOrElse(defaultValue))
      assertResult(2)(counter) // invoked once

      assertResult(99)(Some(99).getOrElse(defaultValue))
      assertResult(2)(counter) // not invoked this time
    } //def

  } //object

}