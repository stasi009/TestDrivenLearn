package tests

import org.scalatest.Spec

sealed class ImmutableSetTest extends Spec {

  object `basic operation` {

    def `test add` = {
      val original = Set(1, 2, 3)

      // if adding a duplicate, then the returned set is just the same as original
      // no new (but identical) copy is made
      val s1 = original + 1
      assert(s1 eq original)

      // make a new copy when adding a new element
      val s2 = original + 9
      assert(original == Set(1, 2, 3)) // not change in place
      assert(s2 == Set(1, 2, 3, 9)) // but return a new but modified copy
    }

    def `test remove` = {
      val original = Set(1 to 5: _*)

      val s1 = original - 1
      assertResult(Set(2, 3, 4, 5))(s1)

      val s2 = original - (3, 4)
      assertResult(Set(1, 2, 5))(s2)
    } //def

    def `test contain` = {
      val s = Set(1, 2, 3)
      assert(s contains 1)
      assert(!(s contains 9))
    }
    
    def `test contains with ()` = {
      val s = Set("a","b","c")
      assert(s("a"))
      assert(!s("x"))
    }//def

    def `test subset` = {
      assert(Set(1, 2) subsetOf Set(1, 2, 3))
      assert(!(Set(0, 2) subsetOf Set(1, 2, 3)))
    } //def

    def `union, intersection, difference` = {
      val s1 = Set(0, 1, 2, 3, 4)
      val s2 = Set(2, 3, 4, 5, 6)

      val union = s1 | s2
      assertResult(Set(0, 1, 2, 3, 4, 5, 6))(union)

      val intersection = s1 & s2
      assertResult(Set(2, 3, 4))(intersection)

      val difference1 = s1 &~ s2
      assertResult(Set(0, 1))(difference1)

      val difference2 = s2 &~ s1
      assertResult(Set(5, 6))(difference2)
    }

  } //object

}