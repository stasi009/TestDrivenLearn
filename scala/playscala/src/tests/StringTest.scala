package tests

import org.scalatest.Spec
import common.Utility

class StringTest extends Spec {

  // *************************** format and concatenate strings
  object `format and concatenate strings` {

    def `concatenate by +` = {
      assertResult("abcxy") { "abc" + "xy" }
      assertResult("abc123") { "abc" + 123 }
    }

    def `format using format method` = {
      assertResult("stasi      9 88.89") { "%-10s %d %3.2f" format ("stasi", 9, 88.888888) }
    }

    def `interpolate with s` = {
      val name = "James"
      assertResult("Hello, James")(s"Hello, $name")

      val a = 1
      val b = 8
      assertResult("1+8=9")(s"$a+$b=${a + b}")
    }

    def `interpolate with f` {
      val height = 1.8
      val name = "James"
      assertResult("James is 1.80 meters tall")(f"$name%s is $height%2.2f meters tall")
      assertResult("     James is   1.80 meters tall")(f"$name%10s is $height%6.2f meters tall")
    }
  }

  // *************************** compare strings
  object `compare strings` {

    def `test equalsIgnoreCase` = {
      val s1 = "stasi"
      val s2 = "Stasi"
      assert(s1 != s2)
      assert(s1.equalsIgnoreCase(s2))
    }

    def `test equality` = {
      val s1 = "hello stasi"
      val s2 = "hello stasi"
      val s3 = new String("hello stasi")

      // 'eq' checks reference equality
      assert(s1 == s2)
      assert(s1 eq s2) // because of the "intern" mechanism

      // '==' call 'equals'
      // by default, 'equals' just call 'eq' to check reference equality
      // however, string overrides 'equals' to provide content equality
      assert(s1 == s3) // content equal
      assert(!(s1 eq s3)) // but points to different object
    }

    def `compare inequality` = {
      assert("a" < "b")
      assert("c".compareTo("b") > 0)
    }
  }

  // *************************** string builder
  object `string builder` {
    def `test string builder` = {
      val sb = new StringBuilder

      // add single character
      sb += 'a'

      // concatenate another string
      sb ++= " lonely heart"

      // convert into string
      assertResult("a lonely heart")(sb.toString)
    }
  }

  // *************************** api demos
  object `api demo` {
    
    def `support Unicode` = {
      val name = "史塔西" // unicode string
      assertResult('塔')(name(1)) // unicode character
      assertResult(3)(name.length)
    }

    def `index within string` = {
      val s = "stasi"
      assertResult('s')(s(0))

      // scala doesn't support negative indices
      intercept[StringIndexOutOfBoundsException] { assertResult('i')(s(-1)) }
      intercept[StringIndexOutOfBoundsException] { assertResult('i')(s(100)) }
    }

    def `to array` = {
      val s = "stasi"
      assertResult(Array('s', 't', 'a', 's', 'i'))(s.toCharArray)
    }

    def `lowercase and uppercase` = {
      assert('S'.isUpper)
      assert('t'.isLower)

      val s = "Stasi"
      assertResult("STASI")(s.toUpperCase)
      assertResult("stasi")(s.toLowerCase)
    }

    def `convert between number and string` = {
      assertResult("10")(10.toString)
      assertResult(9)("9".toInt)

      assert(Utility.numAlmostEqual(9.86, "9.86".toDouble))

      // whitespace will also make the conversion failure
      // we need "trim" first
      intercept[NumberFormatException] { "        9".toInt }
      assertResult(98)("         98 ".trim.toInt)
    }

    def `intersect two strings` = {
      assertResult("lo") { "Hello" intersect "world" }
    }

    def `distinct within one string` = {
      assertResult("abc")("abbac".distinct)
    }

    def `test contains` = {
      val txt = "hello, scala"
      assert(txt contains "sca")
      assert(txt contains ',')
    }

    def `test split` = {
      val segments = "hello,scala" split ","
      assert(segments.isInstanceOf[Array[String]])
      assert(segments sameElements Array("hello", "scala"))
    }

    def `test trim` = {
      assertResult("hello")("            hello         ".trim)
    }

    def `test substring` = {
      val txt = "0123456789"

      val startindex = 3
      // from the start index (included) to the end
      assertResult("3456789")(txt.substring(startindex))

      val endindex = 7 // end is not included
      assertResult("3456")(txt.substring(startindex, endindex))

      // if the endindex is out of boundary
      // an exception will be thrown
      intercept[StringIndexOutOfBoundsException] { txt.substring(8, 100) }
    }

  } // demo api usage

}