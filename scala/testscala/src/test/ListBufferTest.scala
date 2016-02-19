package test

import org.scalatest.Spec
import collection.mutable.ListBuffer

sealed class ListBufferTest extends Spec {

  object `basic operation` {
    def `add elements with +=` = {
      val b = ListBuffer[Int]()

      // add one
      b += 1
      assert(b == List(1))

      // add multiple elements by enclosing them in ()
      b += (8, 9, 10)
      assert(b == List(1, 8, 9, 10))
    }

    def `append collection with ++=` = {
      val b = ListBuffer[Int]()

      b ++= Array(1, 2)
      b ++= Vector(3, 4)

      assert(b == Seq(1, 2, 3, 4))
    }

    def `remove with -= and --=` = {
      val b = ListBuffer(1, 1, 2, 2, 3, 3, 4, 4)

      b -= 1 // different from "remove", 1 here is the element, not the position
      // not remove all matched, just remove the first matched
      assert(b == Seq(1, 2, 2, 3, 3, 4, 4))

      b -= (1, 2, 3)
      assert(b == Seq(2, 3, 4, 4))

      // remove from another collection
      b --= Array(3, 4, 5, 6) // not existing, won't hurt
      assert(b == Seq(2, 4))
    }

  } //object

}