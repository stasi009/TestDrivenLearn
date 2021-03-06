package tests

import org.scalatest.Spec

/*
 * sealed: you can inherit this class, but only in the same source file
 * final: you cannot inherit this class, even in the same source file
 */
sealed class ClassInheritTest extends Spec {

  // ****************************** Person hierarchy ****************************** //
  class Person(val name: String) {
    // must use "override" to override a method that is NOT abstract
    // override an abstract method, "override" modifier is not necessary, but also won't hurt
    override def toString = s"Person[$name]"
  }

  // pay attention that "name" doesn't have "val" before it
  // because that will generate a new copy field and getter/setter
  // just make it a parameter and pass it into the constructor of the parent class
  sealed class Employee(name: String, val salary: Int) extends Person(name) {
    override def toString = super.toString + s", salary: $salary"
  }

  // override member field
  sealed class SecretAgent(val codeName: String) extends Person("") {
    override val name = "secret"
    override def toString = "secret"
  }

  // ****************************** Parent hierarchy ****************************** //
  object Invoke extends Enumeration {
    val Undefined, ByParent, ByChild = Value
  }

  final class Argument {
    private var _invokeBy: Invoke.Value = Invoke.Undefined
    def runByParent() = { _invokeBy = Invoke.ByParent }
    def runByChild() = { _invokeBy = Invoke.ByChild }
    def invokeBy = _invokeBy
  }

  class Parent(val name: String) {
    def Run(arg: Argument) = arg.runByParent()
    override def toString = s"Parent[$name]"
  }

  final class Child(name: String, val id: Int = 9) extends Parent(name) {
    override def Run(arg: Argument) = arg.runByChild()
    override def toString = s"Child[$name]"
  }

  // ****************************** Tests ****************************** //
  object `test override` {

    def `override methods` = {
      val p = new Person("cheka")
      assert(p.toString == "Person[cheka]")

      val e = new Employee("stasi", 100)
      assert(e.name == "stasi")
      assert(e.toString == "Person[stasi], salary: 100")
    }

    def `override fields` = {
      val s = new SecretAgent("lion")
      assert(s.codeName == "lion")
      assert(s.name == "secret")
      assert(s.toString == "secret")
    }

    def `anonymous subclass` = {
      val temp = new Person("a") {
        override def toString = s"Temp<$name>"
      }
      assertResult("a")(temp.name)
      assertResult("Temp<a>")(temp.toString)
    } //def
  } //object

  object `check types` {

    // <child instance>.isInstanceOf[parent type] will return true
    def `test isInstanceOf` = {
      val e = new Employee("a", 8)
      assert(e.isInstanceOf[Person])
      assert(!e.isInstanceOf[SecretAgent])
    } //def

    def `getClass and classOf` = {
      val sa = new SecretAgent("alice")
      assert(sa.isInstanceOf[Person])
      assert(sa.getClass != classOf[Person])
      assert(sa.getClass == classOf[SecretAgent])
    }
  } //object

  object `type cast` {

    def `use asInstanceOf` = {
      val parentRef: Parent = new Child("stasi", 99)
      val a1 = new Argument
      parentRef.Run(a1)
      assert(a1.invokeBy == Invoke.ByChild)

      val childRef: Child = parentRef.asInstanceOf[Child]
      val a2 = new Argument
      childRef.Run(a2)
      assert(a2.invokeBy == Invoke.ByChild)
    }

    def `failed cast` = {
      val p = new Parent("b")
      intercept[ClassCastException] { p.asInstanceOf[Child] }
    }

  } //object

  object `test polymorphism` {
    def getName(p: Person) = p.name

    def `test polymorphism 1` = {
      val people = Seq(new Person("cheka"), new Employee("stasi", 100), new SecretAgent("alpha"))
      val names = people map getName
      assert(names == Seq("cheka", "stasi", "secret"))
    }

    def `test polymorphism 2` = {
      var parentRef: Parent = null

      // ---------------- run on parent
      parentRef = new Parent("cheka")
      val a1 = new Argument
      assert(a1.invokeBy == Invoke.Undefined)

      parentRef.Run(a1)
      assert(a1.invokeBy == Invoke.ByParent)

      // ---------------- run on child
      // child instance can be assigned to parent reference
      parentRef = new Child("stasi", 99)
      val a2 = new Argument

      parentRef.Run(a2)
      assert(a2.invokeBy == Invoke.ByChild)
    } //def
  } //object

}