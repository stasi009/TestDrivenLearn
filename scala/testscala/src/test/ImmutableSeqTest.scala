package test

import org.scalatest.Spec

sealed class ImmutableSeqTest extends Spec {

  object `common api usage` {

    def `append and prepend` = {
      val s = Seq(1, 2, 3)

      // can only append or prepend single element
      // (e1,e2) +: s, or, s :+ (e1,e2), won't work
      // (that will add a tuple, other than flattern and add each one of them)
      assertResult(Seq(0, 1, 2, 3))(0 +: s)
      assertResult(Seq(1, 2, 3, 4))(s :+ 4)
    }

    def `concatenate with another seq` = {
      val s1 = Seq(1, 2, 3)
      val s2 = Seq(4, 5)
      assertResult(Seq(1, 2, 3, 4, 5)) { s1 ++ s2 }
      assertResult(Seq(4, 5, 1, 2, 3)) { s2 ++ s1 }
    }

    def `test diff` = {
      val s1 = Seq(1, 2, 3, 4, 5)
      val s2 = Seq(3, 4, 5, 6, 7)

      assertResult(Seq(1, 2))(s1.diff(s2))
      assertResult(Seq(6, 7))(s2.diff(s1))
    } //def

    def `zip and unzip` = {
      val v1 = Seq(1, 2, 3)
      val v2 = Seq("x", "y", "z")

      // -------------- zip
      val zipped = v1 zip v2
      val expected = Seq((1, "x"), (2, "y"), (3, "z"))
      assertResult(expected)(zipped)

      // -------------- unzip
      // !!! pay attention that, since unzip has an implicit argument
      // !!! we cannot call like "x unzip"
      // !!! we must call like "x.unzip"
      // otherwise, there will be syntax error when decomposing the result tuple
      val (member1, member2) = zipped.unzip
      assert(member1 == v1)
      assert(member2 == v2)
    }

    def `test mkstring` = {
      val s = Seq(1, 2, 3)
      assertResult("123")(s.mkString)
      assertResult("1, 2, 3")(s.mkString(", "))
      assertResult("<1, 2, 3>")(s.mkString("<", ", ", ">"))
    } //def

    def `test correspond` = {
      val s1 = Seq("hello", "scala")
      val s2 = Seq("HELLO", "SCALA")
      assert(s1.corresponds(s2) { _.equalsIgnoreCase(_) })
    }

    def `copy to array` = {
      val array = new Array[Int](5)
      (1 to 3).copyToArray(array, 0)
      assert(Array(1,2,3,0,0) sameElements array)
      
      // inside the destination array, start 3 and only copy 2 elements
      (8 to 20).copyToArray(array,3,2)
      assert(array sameElements Array(1,2,3,8,9))
    } //def

  } //object

  object `Seq, LinearSeq, IndexedSeq objects` {
    def `default implementation` = {

      // Seq defaults to List
      val aseq = Seq(1, 2, 3)
      assert(aseq.isInstanceOf[List[Int]])

      // IndexedSeq defaults to Vector
      val aindexseq = IndexedSeq(1, 2, 3)
      assert(aindexseq.isInstanceOf[Vector[Int]])

      // LinearSeq also defaults to List
      // it seems that "LinearSeq" is not exposed by Predef
      // so we have to use the full name of LinearSeq
      // XXX val alinearseq = LinearSeq(1,2,3)
      val alinearseq = collection.immutable.LinearSeq(1, 2, 3)
      assert(alinearseq.isInstanceOf[List[Int]])

    }
  }

  object `check equality` {

    /*
     * collection equality can cross different types
     * one point to remember is:
     * for primitive types, Array still store them as primitive type
     * while List, Vector, etc, must use 'generic'
     * which still depends on 'type erase', which requires boxing and unboxing
     * maybe that is one reason that why Array cannot equal with Vector, List by content
     */
    def `content-based equality across different types` = {
      val vec = Vector(1, 2, 3)
      val lst = List(1, 2, 3)
      assert(vec == lst)

      val arr = Array(1, 2, 3)
      assert(vec != arr)
    } //def
  } //object

}