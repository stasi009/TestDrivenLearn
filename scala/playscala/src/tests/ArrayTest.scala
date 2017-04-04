package tests

import org.scalatest.Spec

sealed class ArrayTest extends Spec {

  object `slice, view, copy` {

    def `slice return a new copy` = {
      val a = Array(0, 1, 2, 3, 4, 5, 6)

      val subarray = a.slice(from = 1, until = 4)
      assert(subarray sameElements Array(1, 2, 3))

      // since the returned array is a new copy
      // which is separated from the original array
      // so changes are also isolated from each other
      subarray(1) = 1000
      assert(subarray sameElements Array(1, 1000, 3))
      assert(a sameElements Array(0, 1, 2, 3, 4, 5, 6)) // change on slice, original array is not affected

      a(1) = -1
      assert(a sameElements Array(0, -1, 2, 3, 4, 5, 6))
      assert(subarray sameElements Array(1, 1000, 3)) // change on original array, sliced array is not changed
    } //def

    def `view is still connected original array` = {
      val a = Array(0, 1, 2, 3, 4, 5, 6)

      val subview = a.view(from = 1, until = 4)
      assert(subview sameElements Array(1, 2, 3))

      // view still connects to the original array
      // changes will affect each other
      subview(1) = 1000
      assert(subview sameElements Array(1, 1000, 3))
      assert(a sameElements Array(0, 1, 1000, 3, 4, 5, 6)) // change on slice, original array is affected

      a(1) = -1
      assert(a sameElements Array(0, -1, 1000, 3, 4, 5, 6))
      assert(subview sameElements Array(-1, 1000, 3)) // change on original array, sliced array is changed
    }

    def `array class copyToArray` = {
      val src = Array(1, 2, 3)
      val dest = Array(0, 0, 0, 0)

      // the second argument is "start", which is the start position in the destination array
      // for source array, copying always starts from beginning
      src.copyToArray(dest, 2)
      assert(dest sameElements Array(0, 0, 1, 2))
    } //def

    def `array object copy` = {
      val src = Array(1, 2, 3)
      val dest = Array(0, 0, 0, 0)

      Array.copy(src = src,
        srcPos = 1,
        dest = dest,
        destPos = 2,
        length = 2)
      assert(dest sameElements Array(0, 0, 2, 3))
    } //def

  } //object

  object `basic operations` {

    def `access single element` = {
      // to access single element, not use [], but use ()
      val a = Array(1, 2, 3)
      intercept[ArrayIndexOutOfBoundsException] { a(10) }

      // unlike Python, Array doesn't support indexing from backwards
      intercept[ArrayIndexOutOfBoundsException] { a(-1) }

      assertResult(3)(a(2))

      a(1) = -99
      assert(a sameElements Array(1, -99, 3))
    }

    /*
     * Two implicit conversions exist in scala.Predef that are frequently applied to arrays: 
     * 1. a conversion to scala.collection.mutable.ArrayOps
     * 2. a conversion to scala.collection.mutable.WrappedArray (a subtype of scala.collection.Seq). 
     * The conversion to ArrayOps is temporary, as all operations defined on ArrayOps return an Array, 
     * while the conversion to WrappedArray is permanent as all operations return a WrappedArray.
     */
    def `implicit convert to Seq` = {
      def __fool(s: Seq[Int]) = s.length

      val a = Array(1, 2, 3)

      // Array is not Seq
      assert(!a.isInstanceOf[Seq[Int]])

      // however, array can be passed into a function which accepts Seq
      assert(__fool(a) == 3)
    }

    def `initialize arrays` = {
      // use constructor, primitive type are initialized to be zero
      val intarray = new Array[Int](10)
      assertResult(10)(intarray.length)
      assert(intarray forall { _ == 0 })

      // use constructor, reference type are initialized to be null
      val strarray = new Array[String](11)
      assertResult(11)(strarray.length)
      assert(strarray forall { _ == null })

      // use companion object's apply method
      val txts = Array("stasi", "kgb")
      assertResult(2)(txts.length)
      assertResult("stasi")(txts(0))
      assertResult("kgb")(txts(1))
    }

    def `distinguish two constructing ways` = {
      val a1 = Array(10)
      assertResult(1)(a1.length)
      assertResult(10)(a1(0))

      val a2 = new Array[Int](10)
      assertResult(10)(a2.length)
      assert(a2 forall { _ == 0 })
    }

    def `array is mutable` = {
      val original = Array(1, 2, 3)
      val reference = original

      // change in place
      reference(1) = 200
      assertResult(200)(original(1))
      assert(original sameElements Array(1, 200, 3))
    }

    def `to buffer` = {
      val a = Array(1, 2)

      val b = a.toBuffer
      b += (3, 4)

      assert(b sameElements Array(1, 2, 3, 4))
      assert(a sameElements Array(1, 2)) // the original array is not modified
    }

    def `functional sample 1` = {
      val x = Array(1, 2, 3, 4)
      val y = x filter { _ % 2 == 0 } map { _ * 2 }

      // same type as the input collection
      assert(y.isInstanceOf[Array[Int]])
      assert(y sameElements Array(4, 8))
    }

    def `common algorithms` = {
      val a = Array(4, 6, 8, 1, 5)
      assertResult(24)(a.sum)
      assertResult(8)(a.max)
      assertResult(1)(a.min)
    }

    def `test sorted` = {
      val x = Array(4, 6, 8, 1, 5)
      val y = x.sorted
      assert(y sameElements Array(1, 4, 5, 6, 8))

      // y is a new copy which is isolated from original
      y(1) = -99
      assert(y sameElements Array(1, -99, 5, 6, 8))
      assert(x sameElements Array(4, 6, 8, 1, 5))
    }

    def `test sortWith` = {
      val x = Array(4, 6, 8, 1, 5)

      val sortedAscending = x.sortWith(_ < _)
      assert(sortedAscending sameElements Array(1, 4, 5, 6, 8))

      val sortedDescending = x.sortWith(_ > _)
      assert(sortedDescending sameElements Array(8, 6, 5, 4, 1))
    }

    def `sort in place` = {
      val x = Array(4, 6, 8, 1, 5)
      util.Sorting.quickSort(x)
      assert(x sameElements Array(1, 4, 5, 6, 8))
    }

    def `test mkstring` = {
      val s = Array(1, 2, 3)
      assertResult("123")(s.mkString)
      assertResult("1, 2, 3")(s.mkString(", "))
      assertResult("<1, 2, 3>")(s.mkString("<", ", ", ">"))
    }
  }

  object `array equality` {

    // since Array still uses the default "equals"
    // so "==" still checks on "reference equality"
    // test whether two names points to the same object or not
    def `default ==` = {
      val original = Array(1, 2, 3)
      val sameRef = original
      val sameContent = Array(1, 2, 3)

      assert(original == sameRef)
      assert(original != sameContent)
    }

    /**
     * eq is also checking reference equality
     */
    def `eq and equals` = {
      val original = Array(1, 2, 3)
      val sameRef = original
      val sameContent = Array(1, 2, 3)

      assert(original eq sameRef)
      assert(original equals sameRef)
      assert(!(original equals sameContent))
    }

    def `check sameElements` = {
      val original = Array(1, 2, 3)
      val sameElem_sameOrder = Array(1, 2, 3)
      val sameElem_diffOrder = Array(3, 2, 1)

      // not the same object
      assert(original != sameElem_sameOrder)
      // true, when same elements in same order
      assert(original sameElements sameElem_sameOrder)
      // false, when same elements but different order
      assert(!(original sameElements sameElem_diffOrder))
    }
  } // equality check on array

  object `multi-dim arrays` {

    def `matrix-like array` = {
      val (nrows, ncols) = (3, 4)
      val matrix = Array.ofDim[Int](nrows, ncols)
      assert(matrix.length == nrows)

      val (r, c) = (1, 2)
      matrix(r)(c) = 99
      assertResult(99)(matrix(r)(c))
    }

    def `ragged array` = {
      val triangle = new Array[Array[Int]](10)
      for (r <- 0 until triangle.length) {
        triangle(r) = new Array[Int](r + 1)
      }
      assertResult(10)(triangle.length)
      assertResult(1)(triangle(0).length)
      assertResult(10)(triangle(9).length)
    }
  }

}