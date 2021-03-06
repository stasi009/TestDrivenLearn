package test

import org.scalatest.Spec

class PatternMatchTest1 extends Spec {

  object `basic operations` {

    def `simple demo1` = {
      def __fool(ch: Char) = {
        ch match {
          case '+' => 1
          case '-' => -1
          case _ => 0
        } //match
      } // __fool function

      for ((ch, sign) <- Array(('+', 1), ('-', -1), (' ', 0))) {
        assertResult(sign)(__fool(ch))
      } //for
    }

    def `test variable binding 1` = {
      def __fool(ch: Char) = {
        ch match {
          case '+' => "positive"
          case '-' => "negative"
          case ch => s"other character: '$ch'"
        } //match
      } //def 

      assertResult(Seq("positive",
        "other character: 'a'",
        "negative",
        "other character: 'b'")) { Seq('+', 'a', '-', 'b') map __fool }
    } //def

    def `test variable binding 2` = {
      def __fool(n: Int) = {
        n match {
          case x => "always" // 'x' capture all inputs
          case 1 => "1"
          case 2 => "2"
          case 3 => "3"
          case _ => "unknown"
        } //match
      }

      val result = (1 to 3) map __fool
      assert(result forall { _ == "always" })
    }

    def `match with guard` = {
      def sign(x: Int) = {
        x match {
          case _ if x < 0 => -1
          case _ if x > 0 => 1
          case _ if x == 0 => 0
        } // match
      } //def

      assertResult(Seq(-1, 0, 1)) { Seq(-8, 0, 9) map sign }
    } //def

    /*
     * match against constants
     * to prevent the case that 'constant identifier' is mis-understood as 'pattern variable'
     * the constant must start with capital letter
     */
    def `match constants capital letter` = {
      val Positive = 1
      val Negative = 2
      val Zero = 3

      def __fool(flag: Int) = {
        flag match {
          case Positive => "positive"
          case Negative => "negative"
          case Zero => "zero"
          case other => s"unknown flag: $other"
        }
      }

      assertResult("positive")(__fool(1))
      assertResult("negative")(__fool(2))
      assertResult("zero")(__fool(3))
      assertResult("unknown flag: 99")(__fool(99))
    }

    /*
     * if the constants are not started with capital letter
     * then when matching them, we must enclose them with backquotes
     */
    def `match constants backquotes` = {
      val positive = 1
      val negative = 2
      val zero = 3

      def __fool(flag: Int) = {
        flag match {
          case `positive` => "positive"
          case `negative` => "negative"
          case `zero` => "zero"
          case other => s"unknown flag: $other"
        }
      }

      assertResult("positive")(__fool(1))
      assertResult("negative")(__fool(2))
      assertResult("zero")(__fool(3))
      assertResult("unknown flag: 88")(__fool(88))
    }

  } // object

}