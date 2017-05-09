package tests

import org.scalatest.Spec

/**
 * A set of case clauses enclosed in braces is a partial function—
 * a function which may not be defined for all inputs.
 */
sealed class PartialFuncTest extends Spec {

  object `basic operations` {
    val isEven: PartialFunction[Int, String] = { case x if x % 2 == 0 => x + " is even" }
    val isOdd: PartialFunction[Int, String] = { case x if x % 2 == 1 => x + " is odd" }

    def `demo 1` = {
      assertResult("8 is even")(isEven(8))
      assert(isEven.isDefinedAt(6))

      intercept[MatchError] { isEven(9) }
    } //def

    /*
     * write a unmatched partial function
     * will cause compile-time failure, other than runtime error
     * such error will be detected at compile time, other than runtime
    def `use in functional operators` = {
      val mixed = Seq("a" -> 1, "a" -> 2, "a" -> 3,
        "b" -> 4, "b" -> 5,
        "c" -> 6, "c" -> 7, "c" -> 8)
      val grouped = mixed.groupBy { case (s, n, x, y) => s }
    }
    * 
    */

    def `test collect` = {
      val result = (1 to 10) collect isEven
      val expected = (1 to 10) filter { _ % 2 == 0 } map { x => s"$x is even" }
      assertResult(expected)(result)
    }

    def `test orElse` = {
      assert(!isEven.isDefinedAt(9))

      val allfunc = isEven orElse isOdd
      assertResult("9 is odd")(allfunc(9))
    } //def

  } //object

}