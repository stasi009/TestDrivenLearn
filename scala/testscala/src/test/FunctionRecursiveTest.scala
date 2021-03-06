package test

import org.scalatest.Spec
import scala.annotation.tailrec

sealed class FunctionRecursiveTest extends Spec {
  object `recursive functions` {

    // recursive functions must define result type explicitly
    def fact(n: Int): Int = if (n <= 0) 1 else n * fact(n - 1)

    def `simple demo` = {
      assertResult(120)(fact(5))
    }

    def __length[T](lst: List[T]) = {
      @tailrec
      def __length__[T](lst: List[T], length: Int): Int = {
        lst match {
          case Nil => length
          case head :: tail => __length__(tail, length + 1)
        } //match
      }
      __length__(lst, 0)
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

    def `list recursive pattern match` = {
      assertResult(4) { __length(List(3, 4, 5, 6)) }
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