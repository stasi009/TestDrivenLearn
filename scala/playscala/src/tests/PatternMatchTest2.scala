package tests

import org.scalatest.Spec
import scala.annotation.tailrec

sealed class PatternMatchTest2 extends Spec {

  object `match against type` {

    def any2int(obj: Any) = {
      obj match {
        case n: Int => n
        case s: String => s.toInt
        case d: Double => d.round.toInt // round returns Long
        case _ => throw new NotImplementedError
      } //match
    }

    def `demo 1` = {
      assertResult(1)(any2int(1))
      assertResult(101)(any2int("101"))
      assertResult(100)(any2int(99.9))
    } //def
  }

  object `match against collection types` {

    def `match against array` = {
      def __fool(array: Array[Int]) = {
        array match {
          case Array(0) => "0"
          case Array(x, y) => s"[$x,$y]"
          case Array(0, _*) => "0, ..."
          case _ => "else"
        } //match
      }

      assertResult(Seq("0", "[0,1]", "0, ...", "[1,2]", "else")) {
        Seq(Array(0), Array(0, 1), Array(0, 1, 2, 3), Array(1, 2), Array(4)) map __fool
      }
    } //def

    def `match against list` = {
      def __fool(lst: List[Int]) = {
        lst match {
          case 0 :: Nil => "0"
          case x :: y :: Nil => s"[$x,$y]"
          case 0 :: tail => "0, ..."
          case _ => "else"
        } //match
      }

      assertResult(Seq("0", "[0,1]", "0, ...", "[1,2]", "else")) {
        Seq(List(0), List(0, 1), List(0, 1, 2, 3), List(1, 2), List(4)) map __fool
      }
    } //def

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

    def `list recursive pattern match` = {
      assertResult(6) { __length(List(1, 2, 3, 4, 5, 6)) }
    } //def

    def `match against tuple` = {
      def __fool(t: (Int, String)) = {
        t match {
          case (1, "x") => true
          case (_, "*8*") => true
          case (9909, _) => true
          case _ => false
        }
      } //def

      assert(Seq((34, "*8*"), (789, "*8*"), (9909, "x"), (9909, "y"), (1, "x")) forall __fool)

    } //def

  } //object

}