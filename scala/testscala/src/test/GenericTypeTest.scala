package test

import org.scalatest.Spec

sealed class GenericTypeTest extends Spec {

  object `basic demo` {

    sealed class Node[T] {
      private var _value: T = _
      var next: Node[T] = _

      def value = _value
      def value_=(newvalue: T) = _value = newvalue
    }

    sealed class Pair[F, S](val first: F, val second: S) {
      override def equals(other: Any) = {
        other match {
          case otherpair: Pair[F, S] => first == otherpair.first && second == otherpair.second
          case _ => false
        } //match
      } //def

      override def hashCode = (first.toString + second.toString).hashCode
    } //class

    def `test generic type 1` = {
      val intnode = new Node[Int]
      intnode.value = 9
      assert(intnode.value == 9)
      intnode.next = new Node[Int]

      val strnode = new Node[String]
      strnode.value = "scala"
      assert(strnode.value == "scala")
      strnode.next = new Node[String]
    } //def

    def `test generic type 2` = {
      val pair1 = new Pair(1, "stasi") // automatically inference type
      assert(pair1 == new Pair(1, "stasi"))

      val pair2 = new Pair("cheka", 9.9)
    }

  } //object

  object `test boundary` {

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

    /*
     * we must use <%, this is called "view bound"
     * that is because, Int doesn't extend Ordered
     * but it has an implicit conversion which can convert Int to RichInt which extends that trait
     * so we must use "<%" other than "<:"
     * 
     * view bound <% T states that: the parameter can be implicitly converted into T
     */
    def getmin[T <% Ordered[T]](x: T, y: T) = {
      if (x < y) x else y
    }

    def `test upper/view boundary` = {
      // --------------- built in types
      // these two types, actually are based on "view bound"
      assert(getmin(1, 2) == 1)
      assert(getmin("stasi", "kgb") == "kgb")

      // --------------- user-defined type
      assert(getmin(new Ball(1), new Ball(0.5f)) == new Ball(0.5f))
    }
  } //object

  object `test type variance` {

    sealed class Person(val name: String) {
      override def toString = name
    }

    /*
     * Person cannot be a case class here
     * case-to-case inheritance is prohibited, cannot derive one case class from another case class
     * that will cause confusion during pattern match
     */
    sealed class Student(id: Int, name: String) extends Person(name)

    /*
     * "+" makes the "Pair" class covariant
     * which means: this class varies the same direction as its typed parameter
     * since Student is subtype of Person, so Pair[Student] will be subtype of Pair[Person]
     */
    sealed class Pair[+T](val x: T, val y: T)

    // the "variance" feature solves such problem:
    // even if "Student derives from Person", can Pair[Student] be viewed as child of Pair[Person]
    def makeFriends(pair: Pair[Person]) = s"${pair.x} and ${pair.y} become friends"

    def `demo variance feature` = {
      val pair = new Pair(new Student(1, "cheka"), new Student(9, "stasi"))
      val result = makeFriends(pair)
      assert(result == "cheka and stasi become friends")
    } //def

  } //object

}