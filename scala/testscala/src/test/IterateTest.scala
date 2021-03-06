package test

import org.scalatest.Spec

sealed class IterateTest extends Spec {

  object `test iterable` {

    def `default implementation of iterable` = {
      val x = Iterable(1, 2, 3)
      assert(x.isInstanceOf[List[Int]])
    }
  } //object

  object `test iterator` {

    sealed class NumForeverIterator(start: Int) extends Iterator[Int] {
      private var _current = start

      override def hasNext = true
      override def next() = {
        _current += 1
        _current
      } //def

      def current = _current
    } //class
    
    def `how to use iterator` = {
      val it = (1 to 3).iterator
      assert(it.next == 1)
      assert(it.next == 2)
      assert(it.next == 3)
      
      assert(!it.hasNext)
      intercept[NoSuchElementException]{it.next}
    }

    def `demo builtin methods on iterator` = {
      val it = new NumForeverIterator(100)

      val result = it map { _ * 2 } take 3
      assert(result.isInstanceOf[Iterator[Int]]) // result is stil iterator, still lazy

      assert(it.current == 100) // not evaluated yet
      assertResult(Seq(202, 204, 206))(result.toSeq)
      assert(it.current == 103)
    } //def

  } //object

}