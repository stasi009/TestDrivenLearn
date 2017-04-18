package tests

import org.scalatest.Spec
import annotation.tailrec

/*
 * A List is a finite immutable sequence. 
 * They provide constant-time access to their first element as well as the rest of the list, 
 * and they have a constant-time cons operation for adding a new element to the front of the list. 
 * Many other operations take linear time.
 */
sealed class ListTest extends Spec {

  object `add and remove` {

    def `prepend single element` = {
      val lst = List(1, 2, 3)
      assertResult(Seq(0, 1, 2, 3))(0 :: lst)
    } //def

    def `prepend another list` = { // must be another list, other type won't work
      val lst = List(1, 2, 3)
      assertResult(Seq(-1, 0, 1, 2, 3))(List(-1, 0) ::: lst)
      assertResult(Seq(1, 2, 3, 4, 5))(lst ::: List(4, 5))
    } //def

  } //object

  object `recurisve operations` {

    def mylength[T](lst: List[T]) = {

      // here, we CANNOT write as __length[T]
      // otherwise, the compiler will think that we are declaring another generic type
      @tailrec
      def __length(lst: List[T], currentLength: Int): Int = {
        lst match {
          case Nil => currentLength
          case head :: tail => __length(tail, currentLength + 1)
        } //match
      } //def

      __length(lst, 0)
    } //def

    def mysum[T](lst: List[T])(init0: T)(op: (T, T) => T) = {

      // here, we CANNOT write as __sum[T]
      // otherwise, the compiler will think that we are declaring another generic type
      @tailrec
      def __sum(lst: List[T], currentSum: T): T = {
        lst match {
          case Nil => currentSum
          case head :: tail => __sum(tail, op(currentSum, head))
        } //match
      } //def

      __sum(lst, init0)
    }

    def `test length` = {
      assertResult(4)(mylength(List(8, 9, 6, 1)))
    } //def

    def `test sum` = {

      val sum = mysum(List(100, 10, 1))(0) { _ + _ }
      assertResult(111)(sum)

      val txts = List("hello", "world", "from", "scala")
      val concatenated = mysum(txts)("") { _ + " " + _ }
      assertResult(" hello world from scala")(concatenated)

    } //def 

  } //object

}