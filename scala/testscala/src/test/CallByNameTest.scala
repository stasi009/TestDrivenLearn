package test

import org.scalatest.Spec
import scala.annotation.tailrec

sealed class CallByNameTest extends Spec {

  object `call by name` {

    def `contrast - call by value` = {

      var sideeffect = 0 // simulate external environment
      // simulate a process which has side-effect and cost time
      def fool(a: Int) = { sideeffect += 1; a }

      def callbyvalue(enabled: Boolean, b: Int) = if (enabled) b * b else -1

      val numtimes = 5
      for (index <- 1 to numtimes) {
        assertResult(-1) { callbyvalue(false, fool(index)) }
      }
      // even result of fool is not used, it still cause side-effect
      assert(sideeffect == numtimes)
    } //def

    def `contrast - call by name` = {

      var sideeffect = 0 // simulate side-effect
      // simulate a process which cause side-effect and cost time
      def fool(a: Int) = { sideeffect += 1; a }

      // here, b is not a value any more
      // but actually a function which will be lazily evaluated
      // and it is invoked only when necessary
      def callbyname(enabled: Boolean, b: => Int) = if (enabled) b * b else -1

      // ************** argument "b" is a function which will be lazily evaluated
      // ************** no side-effect will be caused when unnecessary
      val numtimes = 5
      for (index <- 1 to numtimes) {
        assertResult(-1)(callbyname(false, fool(index)))
      }
      assertResult(0)(sideeffect)

      // ************** call-by-name will only cause side-effect when necessary
      for (index <- 1 to numtimes) {
        assertResult(index * index) { callbyname(true, fool(index)) }
      }
      // !!!!!!!!!!!!!! pay attention here "side effect" has been totally caused 2*numtimes
      // !!!!!!!!!!!!!! NOT just numtimes
      // !!!!!!!!!!!!!! that is because, inside the "callbyname" function, we use "b*b"
      // !!!!!!!!!!!!!! (b is a function), each call to callbyname, will invoke function "b" twice
      assertResult(2 * numtimes)(sideeffect)
    } //def

    def `mimic and` = {

      def myand(condition1: Boolean, condition2: => Boolean) = if (condition1) condition2 else condition1

      var sideeffect = 0
      def funcHasSideEffect(c: Boolean) = { sideeffect += 1; c }

      assert(!myand(5 < 3, funcHasSideEffect(1 < 2)))
      assert(sideeffect == 0)

      assert(myand(5 > 3, funcHasSideEffect(1 < 2)))
      assert(sideeffect == 1)

    } //def

    /*
     * both "stop" and "expression" are not variable with fixed value
     * but function which will be lazily evaluated, whose result will change from each run
     * although they look like ordinary variables
     */
    @tailrec
    def until(stop: => Boolean)(expression: => Unit): Unit = {
      if (!stop) {
        expression
        until(stop)(expression)
      } //if
    } //def

    def `use lazy-evaluation feature` = {
      var x = 10
      var counter = 0
      
      until(x == 0) {
        x -= 1
        counter += 1
      }
      
      assert(x == 0)
      assert(counter == 10)
    } //def

  } //object

}