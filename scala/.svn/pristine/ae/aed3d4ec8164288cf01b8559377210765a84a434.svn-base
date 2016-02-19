package test

import org.scalatest.Spec
import collection.mutable.ArrayBuffer

/*
 * An implementation of the Buffer class using an array to represent the assembled sequence internally. 
 * Append, update and random access take constant time (amortized time). 
 * Prepends and removes are linear in the buffer size.
 */
sealed class ArrayBufferTest extends Spec {

  object `demo usages` {

    def `pre-allocate memory` = {
      val ab = new ArrayBuffer[Int](10) // just capacity, not real length
      assert(ab.length == 0)

      ab += (6, 7, 8)
      assert(ab.length == 3)
      assert(ab.toArray sameElements Array(6, 7, 8))
    } //def

    def `prepend elements` = {
      val b = ArrayBuffer(1)

      // add one
      10 +=: b
      assert(b sameElements Array(10, 1))

      // add multiple elements by enclosing them in ()
      Seq(7, 8, 9) ++=: b
      assert(b sameElements Array(7, 8, 9, 10, 1))
    }

    def `append elements with +=` = {
      val b = ArrayBuffer[Int]()

      // add one
      b += 1
      assert(b sameElements Array(1))

      // add multiple elements by enclosing them in ()
      b += (8, 9, 10)
      assert(b sameElements Array(1, 8, 9, 10))
    }

    def `append collection with ++=` = {
      val b = ArrayBuffer[Int]()

      b ++= Array(1, 2)
      b ++= Vector(3, 4)

      assert(b sameElements Array(1, 2, 3, 4))
    }

    def `test trim` = {
      val b = ArrayBuffer(1, 2, 3, 4)

      val count = 2
      b.trimEnd(count)

      assert(b sameElements Array(1, 2))
    }

    def `test insert` = {
      val b = ArrayBuffer(1, 2)

      b.insert(1, 99)
      assert(b sameElements Array(1, 99, 2))

      // first argument is the insert position
      // following arguments are elements to be inserted
      b.insert(1, 8, 9, 10)
      assert(b sameElements Array(1, 8, 9, 10, 99, 2))
    }

    def `test remove` = {
      val b = ArrayBuffer(1 to 10: _*)
      assertResult(10)(b.length)

      // remove the elements on the specified position
      b.remove(1)
      assert(b sameElements Array(1, 3, 4, 5, 6, 7, 8, 9, 10))

      // remove multiple elements
      // the first argument is the position
      // the second argument is the count to be removed
      b.remove(1, 4)
      assert(b sameElements Array(1, 7, 8, 9, 10))
    }

    def `remove with -= and --=` = {
      val b = ArrayBuffer(1, 1, 2, 2, 3, 3, 4, 4)

      b -= 1 // different from "remove", 1 here is the element, not the position
      // not remove all matched, just remove the first matched
      assert(b sameElements Array(1, 2, 2, 3, 3, 4, 4))

      b -= (1, 2, 3)
      assert(b sameElements Array(2, 3, 4, 4))

      // remove from another collection
      b --= Array(3, 4, 5, 6) // not existing, won't hurt
      assert(b sameElements Array(2, 4))
    }

    def `to array` = {
      val b = ArrayBuffer(1, 2, 3)
      assertResult(Array(1, 2, 3))(b.toArray)

      // every time when toArray is invoked
      // it will return a new copy of current array
      b += (7, 8)
      assertResult(Array(1, 2, 3, 7, 8))(b.toArray)
    }
  }

}