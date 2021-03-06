package tests

import org.scalatest.Spec
import collection.mutable.ArrayBuffer

sealed class TraitTest1 extends Spec {

  object `demo 1` {

    trait Logger {
      def log(msg: String): Unit
      def info(msg: String) = log("Info: " + msg)
      def warn(msg: String) = log("Warning: " + msg)
      def severe(msg: String) = log("Severe: " + msg)
    }

    trait ForgetLogger extends Logger {
      // for a trait to override another's trait's abstract method
      // !!! we must decorate with "abstract override"
      // !!! both 'abstract' and 'override' are necessary
      abstract override def log(msg: String) = super.log("")
    }

    trait IndexedLogger extends Logger {
      private var _counter = 0

      // "abstract" force this trait must mixin with a trait which has concrete implementation
      abstract override def log(msg: String) = {
        _counter += 1
        // this 'super' isn't parent in inheritance, but 'next' in the calling chain
        // when multiple traits are chained, execute from the last one
        super.log(s"${_counter}-$msg")
      }
    }

    // one guideline to remember is: never use "abstract fields" in the constructor
    trait ShorterLogger extends Logger {
      val maxLength: Int // abstract member field, waiting to be overidden

      abstract override def log(msg: String) = {
        super.log {
          if (msg.length <= maxLength) msg
          else msg.substring(0, maxLength)
        }
      } //def
    }

    /**
     * if you have multiple traits to implement, after 'extend', use 'with'
     * e.g.: class ConsoleLogger extends Logger with Cloneable with Serializable
     */
    sealed class Process extends Logger {
      val _messages = ArrayBuffer[String]()

      def run() {
        info("a")
        warn("b")
        severe("c")
        info("d")
      }

      override def log(msg: String) = { _messages += msg }
      def messages = _messages
    }

    def `test inline trait implementation` = {
      case class Person(name: String, age: Int)
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

    def `basic operation` = {
      val p = new Process
      p.run()
      assertResult(Seq("Info: a", "Warning: b", "Severe: c", "Info: d"))(p.messages.toSeq)
    }

    def `demo mixin 1` = {
      val p = new Process with ForgetLogger
      p.run()
      assert(p.messages forall { _.isEmpty })
    }

    def `demo mixin 2` = {
      // mix in multiple trait, execute from the last one
      val p = new Process with IndexedLogger
      p.run()
      assertResult(Seq("1-Info: a", "2-Warning: b", "3-Severe: c", "4-Info: d"))(p.messages.toSeq)
    }

    def `chained mixin 1` = {
      val p = new Process with IndexedLogger with ShorterLogger { val maxLength = 4 }
      p.run()
      assertResult(Seq("1-Info", "2-Warn", "3-Seve", "4-Info"))(p.messages.toSeq)
    }

    def `chained mixin 2` = {
      val p = new Process with ShorterLogger with IndexedLogger { val maxLength = 4 }
      p.run()
      assertResult(Seq("1-In", "2-Wa", "3-Se", "4-In"))(p.messages.toSeq)
    }

  } //object

}