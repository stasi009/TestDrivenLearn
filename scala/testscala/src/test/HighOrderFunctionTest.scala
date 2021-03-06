package test

import org.scalatest.Spec

sealed class HighOrderFunctionTest extends Spec {

  object `basic demo` {

    def `convert method into function` = {
      def methodTwoArg(x: Int, y: Int) = x + y
      val funcTwoArg = methodTwoArg _
      assertResult(9)(funcTwoArg(4, 5))

      def methodOneTuple(x: (Int, Int)) = x._1 + x._2
      val funcOneTuple = methodOneTuple _
      assertResult(9)(funcOneTuple((4, 5)))
    }

    def `convert class method into function` = {
      class Fool(seed: Int) {
        private var _counter = seed

        def increment() = {
          _counter += 1
          _counter
        } //def
      } //class

      val obj = new Fool(100)
      /*
       * pay attention to the last "_", 
       * with the "_", func will be function
       * otherwise, func will be a Int value
       */
      val func = obj.increment _

      val numbers = for (index <- 1 to 10) yield func()
      assertResult(101 to 110)(numbers)
    } //def

    def `define function literal` = {
      val func1 = (x: Int, y: Int) => x + y
      assert(func1(6, 4) == 10)
    } //def

    def `method automatically wrapped into function` = {
      def fool(x: Int) = x.toString
      // actually "map" needs a function, not a method
      // however, if you pass in a method, that method will be automatically
      // wrappered into a function
      // we don't need to write like "fool _"
      assertResult(Seq("1", "9", "8")) { Seq(1, 9, 8) map fool }
    }

    def `test andThen 1` = {
      sealed abstract class Drawing
      case class Point(x: Int, y: Int) extends Drawing
      case class Circle(p: Point, r: Int) extends Drawing
      case class Cylinder(c: Circle, h: Int) extends Drawing

      // "point" function cannot be written as "point(x:Int,y:Int)"
      // because only "Function1" (which only accept one argument) has the "andThen" function
      def point(x: (Int, Int)) = Point(x._1, x._2)
      def circle(r: Int)(p: Point) = Circle(p, r)
      def cylinder(h: Int)(c: Circle) = Cylinder(c, h)

      val drawall = (point _) andThen (circle(3) _) andThen (cylinder(9) _)

      val cy = drawall((8, 6))
      assert(cy.isInstanceOf[Cylinder])

      val Cylinder(Circle(Point(x, y), r), h) = cy
      assert(x == 8 && y == 6)
      assert(r == 3)
      assert(h == 9)
    } //def

    def `test andThen 2` = {
      // two ways of defining a function
      // [1]: convert a method into a function
      def method1(x: Int) = x + 3
      val func1 = method1 _

      // [2]: define directly a function
      val func2 = (x: Int) => x * 3

      val composed1 = func1 andThen func2
      assertResult(15)(composed1(2))

      val composed2 = func2 andThen func1
      assertResult(9)(composed2(2))
    }

    def `test closure` = {
      var counter = 0
      def __increment() = {
        counter += 1
        counter
      }

      val numbers = for (index <- 1 to 6) yield __increment()
      assertResult(1 to 6)(numbers)
    }

    /*
     * this test tell us, unlike iterator variables in C#, which is a "var"
     * iterator variable in Scala, is a "val"
     * that means, when being captured in loop, every time the closure binds to a new variable
     * which won't be updated during iteration
     */
    def `test closure with loop` = {
      val functions = for (index <- 1 to 3) yield () => index
      val results = functions.map { _() }
      assert(results == Seq(1, 2, 3))
    } //def

  } //object

  object `curry and partial-applied-function` {
    def `explicit define curry function` = {

      def fool(x: Int)(y: Int) = x * y

      val partialfunc = fool(3) _
      assertResult(18)(partialfunc(6))
    } //def

    def `call curry on function` = {

      def method(x: Int, y: Int) = x * y
      val func = method _ // accept two arguments
      val curryfunc = func.curried // accept one argument, and return a one-argument function

      val partialfunc = curryfunc(3)
      assertResult(18)(partialfunc(6))
    } //def

  } //object

  object `function as parameter and result` {

    def `function as input` = {

      def fool(converter: Int => String) = (1 to 3) map converter

      assertResult(Seq("1", "2", "3"))(fool { _.toString })

      assertResult(Seq("one", "two", "three"))(fool {
        case 1 => "one"
        case 2 => "two"
        case 3 => "three"
      })
    } //def

    def `function as output` = {
      def mulby(factor: Int) = (x: Int) => x * factor

      val m1 = mulby(3)
      assert(m1(5) == 15)
      assert(m1(3) == 9)

      val m2 = mulby(4)
      assert(m2(4) == 16)
      assert(m2(10) == 40)
    }

  } //object
}



















