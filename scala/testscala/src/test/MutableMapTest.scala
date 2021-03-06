package test

import org.scalatest.Spec

sealed class MutableMapTest extends Spec {

  object `basic operations` {
    
    def `test contains` = {
      // default implementation is hash map
      val m = collection.mutable.Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8)
      assert(m contains "stasi")
      assert(! (m contains "no such key"))
    }
    
    def `get return optional` = {
      val m = collection.mutable.Map("stasi"->1,"kgb"->9,"gru"->8)
      
      val result1 = m.get("stasi")
      assert(result1.isDefined)
      assert(result1.get == 1)
      
      val result2 = m.get("no such key")
      assert(result2.isEmpty)
    }

    def `add or update with ()` = {
      val m = collection.mutable.Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8)

      // add new key-value pair
      m("cia") = 4

      // update existing key-value pair
      m("stasi") = 100

      // check
      assertResult(4)(m.size)
      val expected = Seq("stasi" -> 100, "kgb" -> 9, "gru" -> 8, "cia" -> 4)
      for ((k, v) <- expected) {
        assert(m(k) == v)
      }
    }

    def `add multiple with += and ++=` = {
      val m = new collection.mutable.HashMap[String,Int]()
      m ++= Array("stasi" -> 1, "kgb" -> 9, "gru" -> 8)

      // update one, 
      // and add two new ones
      m += ("stasi" -> 100, "cia" -> 4, "fbi" -> 6)

      // check
      assert(m.size == 5)
      val expected = Seq("stasi" -> 100, "kgb" -> 9, "gru" -> 8, "cia" -> 4, "fbi" -> 6)
      for ((k, v) <- expected) {
        assert(m(k) == v)
      }
    }

    def `remove by key` = {
      val m = collection.mutable.Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8)

      m -= "stasi"
      m -= "no such key" // remove a non-existing key doesn't matter
      m -= "gru"

      assert(m.size == 1)
      assert(m("kgb") == 9)
    }
    
    def `test getOrElseUpdate` = {
      case class Item(value: Int)
      val map = collection.mutable.Map[Int,Item]()
      def safeGet(id:Int) = {
        map.getOrElseUpdate(id, Item(id))// the second argument is "call-by-name"
      }
      
      val item1 = safeGet(10)
      for (index <- 2 to 4) safeGet(index)
      assert(map.size == 4)
      
      assert(map.contains(10))
      assert(map(10) eq item1)// cached, no new instance is created
    }

  } // object

}