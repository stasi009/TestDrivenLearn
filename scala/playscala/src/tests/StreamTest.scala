package tests

import org.scalatest.Spec

sealed class StreamTest extends Spec {

  object `basic operations` {

    def `demo 1` = {
      def numsFrom(start: Int): Stream[Int] = start #:: numsFrom(start + 1)

      val stream = numsFrom(10)

      val firstpart = stream.take(5).force
      assertResult(10 to 14)(firstpart)

    } //def

    sealed class DataSource {
      private var _sideeffect = 0

      def numsFrom(start: Int): Stream[Int] = {
        _sideeffect += 1
        start #:: numsFrom(start + 1)
      } //def  

      def sideEffect = _sideeffect
    }

    def `demo cache feature of stream` = {
      val ds = new DataSource
      val stream = ds.numsFrom(100)

      // !!! it is INCORRECT to say "stream is always lazy"
      // !!! stream is lazy only for its tail part, but for its head part, stream is always active
      // !!! so after building the stream, stream construction method has already been invoked once
      assertResult(1)(ds.sideEffect)

      // since the head is evaluated one by one
      // so, to access the 5-th element
      // all elements from 0th~4th must also be evaluated, and they are all cached
      assertResult(105)(stream(5))
      assertResult(6)(ds.sideEffect)

      // previous elements are all cached
      // when accessing them, they won't be generated again, so no side effect
      for (index <- 0 to 5)
        assertResult(100 + index)(stream(index))

      assertResult(6)(ds.sideEffect) // previous are cached, no re-generation, no side-effect

      // access new elements, will start the generation process
      assertResult(110)(stream(10))
      assertResult(11)(ds.sideEffect)
    }

    def `collection to stream` = {
      val stream = (1 to 100).toStream
      assertResult(Seq(3, 6, 9)) { (stream map { _ * 3 } take 3).force }
    } //def

  } //object

}