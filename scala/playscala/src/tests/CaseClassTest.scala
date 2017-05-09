package tests

import org.scalatest.Spec

class CaseClassTest extends Spec {

  object `case class polymorphism` {

    /*
     * declare the parent class to be sealed
     * then the compiler can check "match completeness" for us
     */
    sealed trait OsObject
    case class File(name: String) extends OsObject
    case class Process(name: String, priority: Int) extends OsObject
    case object Unknown extends OsObject // singletons object

    def getName(osobj: OsObject) = {
      osobj match {
        case f @ File(name) => name // 'f' represents the "file" object
        case p @ Process(name, priority) => name // 'p' represents the "process" object
        case Unknown => ""
      }
    }

    def `demo 1` = {
      val osobjects = Seq(File("a.txt"), Process("run", 1), Unknown)
      val names = osobjects map getName
      assert(names == Seq("a.txt", "run", ""))
    } //def

  } //object

  object `demo usage` {
    case class Fool(number: Int, name: String)

    // demo that "case class" can also extends trait or abstract classes
    case class Item(v: Int) extends Ordered[Item] {
      override def compare(other: Item) = v - other.v
    } //class

    def `companion object apply` = {
      val f1 = Fool(1, "abc")
      assertResult(1)(f1.number)

      val f2 = Fool(name = "cd", number = 9) // use named arguments
      assertResult("cd")(f2.name)
    }

    def `companion object unapply` = {
      val Fool(number, name) = Fool(1, "xy")
      assert(number == 1)
      assert(name == "xy")
    }

    def `auto value-based equality check` = {
      val f1 = Fool(1, "abc")
      val f2 = Fool(1, "abc")

      assert(!(f1 eq f2))
      assert(f1 == f2)
    }

    def `test toString` = {
      val f = Fool(1, "abc")
      assertResult("Fool(1,abc)")(f.toString)
    }

    def `use copy to modify` = {
      val f1 = Fool(1, "abc")
      val f2 = f1.copy(number = 99)

      assert(f2 == Fool(99, "abc"))
      assert(f1 == Fool(1, "abc")) // f1 remains the same
    }
    
    def `demo case class can extend` = {
      val numbers = Seq(1,9,8,4)
      val balls = numbers map {Item(_)}
      
      val expected = numbers.sorted map{Item(_)}
      assert(balls.sorted == expected)
    }//def
  } //object

}