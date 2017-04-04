package tests

import org.scalatest.Spec

sealed class ObjectTest extends Spec {

  object `singleton object` {

    object Accounts {
      private var lastid = 0
      def newId = { lastid += 1; lastid }
    }

    def `demo singleton 1` = {
      val ids = for (index <- 1 to 3) yield Accounts.newId
      assert(ids == Seq(1, 2, 3))
    }
  }

  object `companion object demo1` {

    // 'private' make the primary constructor private
    // which make the user can only construct the instance through
    // companion object's factory method
    sealed class Account private (val balance: Int) {
      val id = Account.newId // companion object and class can access each other's private fields
    } //class

    object Account {
      private[this] var lastid = 0
      private def newId = { lastid += 1; lastid }

      // factory pattern
      def apply(balance: Int) = new Account(balance)
    } // object

    def `demo companion object` = {
      val balances = Seq(6, 9, 8)
      val accounts = balances map { Account(_) }

      val ids = accounts map { _.id }
      assert(ids == Seq(1, 2, 3))

      val actualbalances = accounts map { _.balance }
      assert(actualbalances == balances)
    } //def
  } //object

  object `companion object factory pattern` {
    sealed class Marker private (val color: String)

    object Marker {
      private val _colors = new collection.mutable.HashMap[String, Marker]

      def apply(color: String) = {
        (_colors get color) match {
          case Some(m) => m
          case None => { val newmarker = new Marker(color); _colors(color) = newmarker; newmarker }
        } //match
      }
    } //object

    def `test factory pattern` = {
      val r1 = Marker("red")
      assert(r1.color == "red")

      val b = Marker("blue")
      assert(b.color == "blue")

      val r2 = Marker("red")
      assert(r1 eq r2) // same reference
    } //def
  }

  object `object extends class` {

    abstract class UndoableAction(val description: String) {
      def undo(): Unit
      def redo(): Unit
    }

    // a useful default is "do nothing", which we only need one of them
    // so it is reasonable to let one object extends the abstract class
    // and share that "singleton" object where it is necessary
    object DoNothingAction extends UndoableAction("do nothing") {
      override def undo() = {}
      override def redo() = {}
    }

    def `share singletion object` = {
      // it is save to share a "do nothing" singleton object 
      val actions = Map("open" -> DoNothingAction, "save" -> DoNothingAction)
    }

  } //object 
}