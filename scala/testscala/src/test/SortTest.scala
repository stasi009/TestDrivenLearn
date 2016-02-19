package test

import org.scalatest.Spec

sealed class SortTest extends Spec {

  object `sort with Ordered trait` {

    sealed class Ball(val radius: Float) extends Ordered[Ball] {
      // compare must return an integer, so we must use "round"
      override def compare(other: Ball) = (radius - other.radius).round

      // when using "==" to invoke "equals"
      // "==" will always check whether the right operand is null or not
      // so we can save the effort, and don't need to check whether the input is null or not
      override def equals(other: Any) = {
        other match {
          case otherball: Ball => (radius - otherball.radius).abs < 1e-6
          case _ => false
        } //match
      } //def

      override def hashCode = radius.round
    }

    def `test comparing operators` = {
      val b1 = new Ball(1.1f)
      val b2 = new Ball(2.2f)

      assert(b1 < b2)
      assert(b2 >= b1)
      assert(b1 == new Ball(1.1f))
    }

    def `sorted is based on Ordered trait` = {
      val numbers = Seq(1, 9, 8, 4)
      val balls = numbers map { new Ball(_) }

      val sortedballs = balls.sorted
      val expected = numbers.sorted.map { new Ball(_) }

      assert(sortedballs == expected)
    } //def

    def `util sorting is based on Ordered trait` = {
      val numbers = Array(1, 9, 8, 4)
      val balls = numbers map { new Ball(_) }

      util.Sorting.quickSort(balls)
      val expected = numbers.sorted map { new Ball(_) }

      assert(balls sameElements expected)
    } //def

  } //object

  object `simple demo` {

    case class Person(id: Int, name: String)

    /*
     * sort with implicit ordering in ascending order
     * also output has the same type as the input
     */
    def `test sorted` = {
      val a = Array(3, 2, 1)
      val aa = a.sorted
      assert(aa.isInstanceOf[Array[Int]])
      assert(aa sameElements Array(1, 2, 3))

      val b = Seq(3, 2, 1)
      val bb = b.sorted
      assert(bb.isInstanceOf[Seq[Int]])
      assertResult(Seq(1, 2, 3))(bb)
    }

    def `test sorted by` = {
      val original = Seq(Person(9, "gru"), Person(1, "stasi"), Person(6, "kgb"))

      val sortedById = original.sortBy { _.id }
      assert(sortedById == Seq(Person(1, "stasi"), Person(6, "kgb"), Person(9, "gru")))

      val sortedByName = original.sortBy(_.name)
      assert(sortedByName == Seq(Person(9, "gru"), Person(6, "kgb"), Person(1, "stasi")))
    }

    def `test sorted with` = {
      val original = Seq(Person(9, "gru"), Person(1, "stasi"), Person(6, "kgb"))

      val sortedById = original.sortWith { _.id < _.id }
      assert(sortedById == Seq(Person(1, "stasi"), Person(6, "kgb"), Person(9, "gru")))

      val sortedByName = original.sortWith { _.name < _.name }
      assert(sortedByName == Seq(Person(9, "gru"), Person(6, "kgb"), Person(1, "stasi")))

    } //def
  }

  object `util Sorting class` {
    def `sort in place` = {
      val x = Array(4, 6, 8, 1, 5)
      util.Sorting.quickSort(x)
      assert(x sameElements Array(1, 4, 5, 6, 8))
    }
  }

}