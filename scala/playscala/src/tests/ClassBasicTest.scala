package tests

import org.scalatest.Spec
import beans.BeanProperty

sealed class ClassBasicTest extends Spec {

  object `getter and setter` {

    def `access privilege` = {
      class Fool {
        /*
         * when you don't specify any modifier, everything is public. 
         * Scala doesn't provide any modifier to mark members as public
         */
        var defaultVarField = 0
        val defaultValField = 1
        private var privateVarField = 0
      }

      val f = new Fool
      // "val field" only define getter
      // no setter is defined for "val field"
      assert(f.defaultValField == 1)
      // f.defaultValField = 100

      // both public getter and public setter are defined for "default var field"
      f.defaultVarField = 100
      assert(f.defaultVarField == 100)

      // "private var field"'s getter and setter are both private
      // cannot be accessed outside the class
      // f.privateVarField

    } //def

    def `auto generated by compiler` = {

      /*
       * the compiler will automatically generate two methods for "var" field
       * value(): for getter
       * value_=(newvalue): for setter
       */
      class Fool {
        var value = 0 // default access level is public
      }

      val f = new Fool
      assert(f.value == 0) // call f.value() for getting purpose

      f.value = 99 // call f.value_=(99) for setting purpose
      assert(f.value == 99)

    } // def

    def `redefine getter and setter` = {
      class Person {
        private var _age = 0

        // getter
        def age = _age

        // setter
        def age_=(newvalue: Int) = {
          if (newvalue < _age) throw new IllegalArgumentException
          else _age = newvalue
        }
      } //class

      val p = new Person
      p.age = 5
      assert(p.age == 5)

      intercept[IllegalArgumentException] { p.age = 1 }
    }

    /*
     * a field can only be changed inside the class
     * but can be visible outside the class
     */
    def `field can only be changed inside the class` = {
      class Person(private var _age: Int = 0) {
        def increment() = _age += 1
        def age = _age
      }
      val p = new Person()
      p.increment()
      assert(p.age == 1)
    }

    def `generate bean property` = {
      class Person(@BeanProperty var name: String)

      val p = new Person("stasi")

      // use scala way to access
      p.name = "gru"
      assert(p.name == "gru")

      // use java bean's way to access
      p.setName("kgb")
      assert(p.getName() == "kgb")

    } //def

  } //object

  object `test constructor` {

    def `primary constructor 1` = {
      class Person(val id: Int, val name: String, var tag: String) //class

      // val: generates a field and getter
      // var: generates a field and both getter and setter
      val p = new Person(1, "cheka", "")
      assert(p.id == 1)
      assert(p.name == "cheka")

      p.tag = "russian"
      assert(p.tag == "russian")
    } //def

    def `primary constructor 2` = {
      var counter = 0

      class Fool(value: Int) {
        // this is the body of the primary constructor
        // which will be executed during the construction
        val square = value * value
        counter += 1

        override def toString() = s"$value * $value = $square"
      }

      val f = new Fool(3)
      assert(f.square == 9)
      assert(f.toString == "3 * 3 = 9")
      assert(counter == 1)

      for (index <- 1 to 10) new Fool(index)
      assert(counter == 11)
    }

    def `no val nor var won't be visible outside` = {
      /*
       * if no var nor val is before the field
       * then those fields are treated as "private"
       * then its getter and setter are private, which are non-visible outside the class
       */
      class Fool(id: Int, name: String)

      val f = new Fool(1, "cheka")
      // f.id
      // f.name
    } //def

    def `auxiliary constructor 1` = {
      class DbClient(val host: String, val port: Int) {
        // auxiliary constructor is called 'this'
        // and it must start with a call to primary constructor or other auxiliary constructor
        def this() = this("localhost", 7027)
      }

      val client = new DbClient
      assert(client.host == "localhost")
      assert(client.port == 7027)
    } //def

    def `auxiliary constructor 2` = {
      sealed class Person {
        // this class doesn't define any primary constructor
        private var _name = ""
        private var _age = 0

        def this(name: String) = {
          this() // must start with a call to primary constructor
          _name = name
        }

        def this(name: String, age: Int) = {
          this(name) // start with a call to other auxiliary constructor
          _age = age
        }

        def name = _name
        def age = _age
      } //class

      val p1 = new Person
      assert(p1.name.isEmpty)
      assert(p1.age == 0)

      val p2 = new Person("cheka")
      assert(p2.name == "cheka")
      assert(p2.age == 0)

      val p3 = new Person("stasi", 9)
      assert(p3.name == "stasi")
      assert(p3.age == 9)
    }

    def `constructor with default value` = {
      sealed class Person(val age: Int = 0, val name: String = "")

      val p1 = new Person
      assert(p1.name.isEmpty)
      assert(p1.age == 0)

      val p2 = new Person(name = "cheka")
      assert(p2.name == "cheka")
      assert(p2.age == 0)

      val p3 = new Person(age = 9, name = "stasi")
      assert(p3.name == "stasi")
      assert(p3.age == 9)
    } //def

  } //object
}