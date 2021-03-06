package tests

import org.scalatest.Spec

sealed class FuncOperatorsTest extends Spec {

  object `test operators` {

    def `test sliding` = {
      val numbers = 1 to 6
      val it = numbers.sliding(size = 3, step = 2)

      assertResult(1 to 3)(it.next)
      assertResult(3 to 5)(it.next)
      assertResult(Seq(5, 6))(it.next)
    } //def

    def `test grouped` = {
      val numbers = 1 to 5
      val it = numbers grouped 3

      assertResult(1 to 3)(it.next)
      assertResult(Seq(4, 5))(it.next)
      intercept[NoSuchElementException] { it.next }
    } //def

    def `test groupBy` = {
      val mixed = Seq("a" -> 1, "a" -> 2, "a" -> 3,
        "b" -> 4, "b" -> 5,
        "c" -> 6, "c" -> 7, "c" -> 8)
      val grouped = mixed.groupBy { case (s, n) => s }
      assert(grouped.size == 3)

      // the value is still a tuple
      // the API has no way to only keep the value part
      assert(grouped("a") == Seq("a" -> 1, "a" -> 2, "a" -> 3))
      assert(grouped("b") == Seq("b" -> 4, "b" -> 5))
      assert(grouped("c") == Seq("c" -> 6, "c" -> 7, "c" -> 8))
    }

    def `uniform return type principle` = {
      // map over an array return an array
      val resultarray = Array(1, 2, 3) map { _ * 2 }
      assert(resultarray.isInstanceOf[Array[Int]])

      // map over a vector return a vector
      val resultvector = Vector(1, 2, 3) map { _ * 2 }
      assert(resultvector.isInstanceOf[Vector[Int]])
    } //def

    def `count, forall, exists` = {
      val a = Array(1, 9, 8, 6, 4, 3, 0)

      assertResult(4) { a count { _ % 2 == 0 } }
      assert(!(a forall { _ % 2 == 0 }))
      assert(a forall { _ >= 0 })

      assert(a exists { _ % 2 == 0 })
      assert(!(a exists { _ < 0 }))
    } //def

    def `test partition` = {
      val (evens, odds) = (1 to 10) partition { _ % 2 == 0 }
      assertResult(2 to 10 by 2)(evens)
      assertResult(1 to 9 by 2)(odds)
    }

    def `take, drop, splitAt` = {
      val sq = Seq(1, 9, 8, 6, 4, 3, 0)

      assertResult(Seq(1, 9))(sq take 2)
      assertResult(Seq(8, 6, 4, 3, 0))(sq drop 2)

      val (firstpart, secondpart) = sq splitAt 2
      assertResult(Seq(1, 9))(firstpart)
      assertResult(Seq(8, 6, 4, 3, 0))(secondpart)
    }

    def `indexOf,lastIndexOf` = {
      val a = Array(1, 9, 8, 6, 4, 3, 1, 0)
      assertResult(0)(a indexOf 1)
      assertResult(6)(a lastIndexOf 1)
    }

    def `test zip and zipWithIndex` = {
      // segments can have different length
      // zipped result has the shortest length among segments
      val ids = 1 to 10
      val names = Seq("a", "b", "c")
      assertResult(Seq((1, "a"), (2, "b"), (3, "c")))(ids zip names)

      assertResult(Seq(("a", 0), ("b", 1), ("c", 2)))(names zipWithIndex)
    } //def

    def `test map` = {
      assertResult(Seq("1", "2", "3")) { (1 to 3) map { _.toString } }
    }

    def `test flatmap` = {
      val names = Seq("stasi", "kgb")
      val characters = names flatMap { _.toCharArray }
      assertResult(Seq('s', 't', 'a', 's', 'i', 'k', 'g', 'b'))(characters)
    }
    
    /**
     * If you use flatMap with a function that returns an Option, 
     * resulting collection contains all values v for which the function returns Some(v).
     */
    def `test flatMap Option` = {
      val names = Seq("stasi", "kgb")
      val result = names.zipWithIndex flatMap { case (name,idx) => if(idx % 2 ==0) Some(name) else None }
      assertResult(Seq("stasi"))(result)
    }

    /**
     * 'collect' yields a collection of all function values of the arguments on which it is defined
     */
    def `test collect` = {
      val characters = Seq('a', 'b', 'c', 'c', 'b', 'a')
      assertResult(Seq(1, -1, -1, 1)) { characters collect { case 'a' => 1; case 'b' => -1 } }
    } //def

    def `test filter` = {
      val actual = (1 to 10) filter { _ % 2 == 0 }
      val expected = Seq(2, 4, 6, 8, 10)
      assertResult(expected)(actual)
    }

    def `test fold 1` = {
      val sbFromLeft = (1 to 5).foldLeft(new StringBuilder) { (sb, n) => sb ++= n.toString }
      assertResult("12345")(sbFromLeft.toString)

      val sbFromRight = (1 to 5).foldRight(new StringBuilder) { (n, sb) => sb ++= n.toString }
      assertResult("54321")(sbFromRight.toString)
    }

    def `test fold 2` = {
      val sbFromLeft = ((new StringBuilder) /: (1 to 5)) { (sb, n) => sb ++= n.toString }
      assertResult("12345")(sbFromLeft.toString)

      val sbFromRight = (((1 to 5) :\ new StringBuilder)) { (n, sb) => sb ++= n.toString }
      assertResult("54321")(sbFromRight.toString)
    }

    def `test scan` = {
      val intermediates1 = (1 to 5).scanLeft(0) { _ - _ }
      assertResult(Vector(0, -1, -3, -6, -10, -15))(intermediates1)

      val intermediates2 = (1 to 5).scanRight(0) { _ - _ }
      assertResult(Vector(3, -2, 4, -1, 5, 0))(intermediates2)
    }

    /*
     * the difference between reduce and fold is that
     * fold can accept a user-defined initial state
     * while reduce use the first element as the initial state
     */
    def `test reduce 1` = {
      val rleft = (1 to 5) reduceLeft { _ - _ } //current state is the first argument
      assertResult(-13)(rleft)

      val rright = (1 to 5) reduceRight { _ - _ } //current state is the second argument
      assertResult(3)(rright)
    }

    def `test sort` = {
      val txts = Seq("abc", "xy")

      // default sort, sort by alphabetic order
      assertResult(Seq("abc", "xy")) { txts.sorted }

      // user-defined sort
      assertResult(Seq("xy", "abc")) { txts.sortBy(_.length) }
      assertResult(Seq("abc", "xy")) { txts.sortWith { _.length > _.length } }
    }

  } //object

}
