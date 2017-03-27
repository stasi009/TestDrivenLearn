package test

import common.Utility
import org.scalatest.Spec

/*
 * Vector is a collection type that addresses the inefficiency for random access on lists. 
 * Vectors allow accessing any element of the list in effectively constant time
 * 
 * algorithms using vectors do not have to be careful about accessing just the head of the sequence. 
 * They can access and modify elements at arbitrary locations
 * 
 * Vectors are represented as trees with a high branching factor 
 * So for all vectors of reasonable size, an element selection involves up to 5 primitive array selections.
 * 
 * Updating an element in the middle of a vector can be done by copying the node that contains the element, 
 * and every node that points to it, starting from the root of the tree
 * This is certainly more expensive than an in-place update in a mutable array, 
 * but still a lot cheaper than copying the whole vector
 * 
 * vectors strike a good balance between fast random selections and fast random functional updates
 */
sealed class VectorTest extends Spec {

  object `demo usages` {
    
    def `append and prepend` = {
      val s = Vector(1, 2, 3)

      // can only append or prepend single element
      // (e1,e2) +: s, or, s :+ (e1,e2), won't work
      // (that will add a tuple, other than flattern and add each one of them)
      assertResult(Seq(0, 1, 2, 3))(0 +: s)
      assertResult(Seq(1, 2, 3, 4))(s :+ 4)
    }

    def `type inheritance` = {
      val v = Vector(6, 8, 9)
      assert(v.isInstanceOf[IndexedSeq[Int]])
      assert(v.isInstanceOf[Seq[Int]])
    }

    def `access by index` = {
      val v = Vector(1, 2, 3)
      assertResult(2)(v(1))
      intercept[IndexOutOfBoundsException] { v(-1) } // negative index is not supported
      intercept[IndexOutOfBoundsException] { v(100) }
    }

    def `not has head::tail structure` = {
      def __fool[T](s: Seq[T]) = s match {
        case head :: tail => head
      } //match

      assertResult(10)(__fool(Seq(10, 9, 8)))

      // although vector is Seq, but it cannot match against "head::tail"
      val v = Vector(11, 101, 1001)
      intercept[MatchError] { __fool(v) }

    } //def

  }

}