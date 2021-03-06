package test

import org.scalatest.Spec
import util.Sorting

sealed class SortWithOrderTest extends Spec {
  /*
   * Ordered should be used for data with a single, natural ordering (like integers) 
   * while Ordering allows for multiple ordering implementations. 
   * scala.math.Ordering is an alternative to this trait that allows multiple orderings 
   * to be defined for the same type.
   */
  object `demo Ordered trait` {
    case class Person(name: String, age: Int) extends Ordered[Person] {
      // now, the sorting strategy is fixed, Person can only be sorted by age
      override def compare(other: Person) = { age - other.age }
    }

    def `Ordered derives from Comparable` = {
      val p1 = Person("a", 10)
      val p2 = Person("b", 4)

      assert(p1.isInstanceOf[Comparable[Person]])
      assert(p1.compare(p2) == 6)
      assert(p1.compareTo(p2) == 6)
    }

    def `test comparing operators` = {
      val p1 = Person("z", 9)
      val p2 = Person("a", 12)
      assert(p2 > p1) // comparing based on age only
      assert(p1 < p2)
    } //def

    def `test sorted` = {
      val people = {
        val ages = Seq(1, 9, 8, 4)
        val names = Seq("a", "b", "c", "d")
        // we must use "case" to define a "partial function" here
        // because "map" here expects a one-argument tuple
        // if we ignore "case", then the compiler see a "two argument function"
        (names zip ages) map { case (name, age) => Person(name, age) }
      }
      val sortedpeople = people.sorted // with implicit ordering strategy

      val expected = people.sortBy { _.age } // consistent with how Person extends trait
      assert(sortedpeople == expected)
    }

    def `sort in place` = {
      val people = Array(Person("a", 1), Person("b", 9), Person("c", 8), Person("d", 4))
      Sorting.quickSort(people)

      val expected = Array(Person("a", 1), Person("d", 4), Person("c", 8), Person("b", 9))
      assert(people sameElements expected)
    } //def

  } //object

  /*
   * difference between Ordered and Ordering is that: 
   * Ordered is a property of the element: if element has "ordered" property, then it can be sorted
   * Ordering is a strategy. Even the element doesn't have "ordered" property, we can design a strategy
   * to sort them, or, develop several sorting strategy
   * 
   * ordered is static: when the element type is fixed, its Ordered property is also fixed
   * each item type can only have one Ordered property
   * 
   * Ordering is dynamic, which is detached from the element type. 
   * one element type can have several different Ordering strategy, and can be switched at runtime
   */
  object `demo Ordering trait` {

    case class Person(name: String, age: Int)

    def `test builtin ordering` = {
      val pairs = Seq(("a", 5, 2), ("c", 3, 1), ("b", 1, 1))

      // sort by 2nd element
      val sorted1 = pairs.sorted(Ordering.by[(String, Int, Int), Int](_._2))
      assertResult(List(("b", 1, 1), ("c", 3, 1), ("a", 5, 2)))(sorted1)

      // sort by the 3rd element, then 1st
      val ord = Ordering[(Int, String)].on[(String, Int, Int)](x => (x._3, x._1))
      val sorted2 = pairs.sorted(ord)
      assertResult(List(("b", 1, 1), ("c", 3, 1), ("a", 5, 2)))(sorted2)
    }

    def `user-defined ordering` = {
      val people = Seq(Person("a", 1), Person("b", 9), Person("c", 8), Person("d", 4))

      val orderByAge = new Ordering[Person] {
        override def compare(p1: Person, p2: Person) = p1.age - p2.age
      }
      assertResult(people.sortBy { _.age })(people.sorted(orderByAge))

      val orderByName = new Ordering[Person] {
        override def compare(p1: Person, p2: Person) = p1.name compare p2.name
      }
      assertResult(people.sortBy { _.name })(people.sorted(orderByName))
    } //def

  }
  //object

}
























