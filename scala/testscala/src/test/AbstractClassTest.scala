package test

import org.scalatest.Spec

/*
 * one rule which should always be remembered:
 * don't call abstract methods in the constructor
 * even though there is some scala syntax sugar allow us to do that (eg. early definition)
 * however, I think, avoding call abstract in the constructor
 * is a even better idea, and a good convention to follow
 */
sealed class AbstractClassTest extends Spec {

  object `demo abstract fields` {

    abstract class AbstractParent {
      // abstract member fields
      val number: Int
      var name: String

      // template methods
      def getMessage = s"$name-$number"
    } //class

    // to override abstract members, "override" isn't necessary
    sealed class ConstantChild(val number: Int, var name: String) extends AbstractParent

    sealed class GenericChild[T](private val _array: T*) extends AbstractParent {
      val number = _array.length

      // explicitly override getter and setter
      def name = _array.mkString(",")
      def name_=(newvalue: String) = throw new NotImplementedError
    }

    def `demo 1` = {
      val c: AbstractParent = new ConstantChild(9, "cheka")
      assertResult(9)(c.number)
      assertResult("cheka-9")(c.getMessage)

      c.name = "stasi"
      assertResult("stasi-9")(c.getMessage)
    }

    def `demo 2` = {
      val g: AbstractParent = new GenericChild[String]("hello", "stasi", "from", "scala")
      assert(g.number == 4)
      assertResult(g.name)("hello,stasi,from,scala")

      intercept[NotImplementedError] { g.name = "cannot" }
      assertResult("hello,stasi,from,scala-4")(g.getMessage)
    }
  } //object

  object `demo abstract methods` {

    /*
     * 1. abstract methods cannot be private
     * 2. when overriding, it can only enhance the access level, cannot lower down it
     * (for example, abstract method in parent can be protected, 
     * when overriding it in child class, the method can be protected or public, just 
     * cannot be private)
     */
    abstract class AbstractParent {
      // abstract member methods
      protected def process(s: String): Int
      protected def process(n: Int): Int

      // template methods
      def run(s: String, n: Int) = {
        val v1 = process(s)
        val v2 = process(n)
        v1 * v2
      }
    }

    // no need to use "override" when override an abstract method
    sealed class Child1 extends AbstractParent {
      protected def process(s: String) = s.length
      protected def process(n: Int) = n
    }

    // no need to use "override" when override an abstract method
    sealed class Child2 extends AbstractParent {
      protected def process(s: String) = s.toInt
      protected def process(n: Int) = -n
    }

    def getProduct(a: AbstractParent, s: String, n: Int) = a.run(s, n)

    def `demo 1` = {
      val c1 = new Child1
      val c2 = new Child2

      val s = "100"
      val n = 9

      assertResult(27)(getProduct(c1, s, n))
      assertResult(-900)(getProduct(c2, s, n))
    }

  } //object

}