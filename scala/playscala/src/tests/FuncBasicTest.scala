package tests

import org.scalatest.Spec

sealed class FuncBasicTest extends Spec {

  object `other features` {

    def `no argument vs. empty argument` = {

      def noArgs = 99
      def emptyArgs() = 99

      val number1: Int = noArgs
      assert(number1 == 99)
      val number2: Int = emptyArgs
      assert(number2 == 99)

      val fun1: () => Int = noArgs _
      val fun2: () => Int = emptyArgs _

    } //def

  } //object

  object `overload issue` {
    def sum(a: Int, b: Int) = a + b
    def sum(a: Int, b: Int, c: Int) = a + b + c
    def sum(a: Double, b: Double) = a + b
  }

  object `arguments issues` {

    def `reference argument, change in place` = {
      def fool(a: Array[Int]) = {
        a(0) = -a(0)
      }

      val a = Array(1, 2, 3)
      fool(a)

      assert(a sameElements Array(-1, 2, 3))
    } //def

    /*
     * we don't need to worry that "reference can be pointed to another piece of memory"
     * because all arguments are "val", which cannot be changed inside the function
    def `reference arguments, point to new memory` = {
      def fool(a: Array[Int]) = {
        a = a map { _ => -_ }
      } //def
    } //def
    */

    def decorate(content: String, left: String = "[", right: String = "]") = left + content + right

    def `test default arguments` = {
      assertResult("[stasi]") { decorate("stasi") }
      assertResult("<<stasi>>") { decorate("stasi", "<<", ">>") }
    }

    def `pass arguments by name` = {
      assertResult("<stasi>") {
        decorate(left = "<", content = "stasi", right = ">")
      }
    }

    // args's type is Seq
    def sum(args: Int*) = {
      var result = 0
      for (i <- args) result += i
      result
    }

    def `arguments with variable length` = {
      assertResult(10) { sum(1, 2, 3, 4) }

      // if a sequence is passed, we have to use "_*" to decompose it
      assertResult(10) { sum(1 to 4: _*) }
    }

    /*
     * when call a java method with variable arguments with type 'object'
     * we have to convert any primitive type by hand
     */
    def `call java method with variable arguments` = {
      // below code won't compile, because it needs variable 'object' arguments
      // XXX val s = String.format("hello %d", 3)
      assertResult("hello 9") { String.format("hello 9", 9: Integer) }
      assertResult("hello 9") { String.format("hello 9", 9.asInstanceOf[Object]) }
    } //def 
  }
}