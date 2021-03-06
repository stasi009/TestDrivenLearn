package tests

import org.scalatest.Spec
import java.util.NoSuchElementException

sealed class ImmutableMapTest extends Spec {

  object `basic operations` {

    def `initialize a map` = {
      // initialize with existing key-value pairs
      val m1 = Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8)
      assertResult(3)(m1.size)

      // by default, Map will return a immutable one
      assert(m1.isInstanceOf[collection.immutable.Map[String, Int]])

      // another way to initialize
      val m2 = Map(("stasi", 1), ("kgb", 9), ("gru", 8))
      assertResult(3)(m2.size)

      // create an empty map
      val m3 = new collection.immutable.HashMap[String, Int]
      assertResult(0)(m3.size)
    }

    def `pairs to map` = {
      val keys = Array("stasi", "kgb", "gru")
      val values = Array(1, 9, 8)
      val map = keys.zip(values).toMap

      for (index <- 0 until keys.length) {
        assert(map(keys(index)) == values(index))
      } //for
    }

    def `get by ()` = {
      val m = Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8)
      assertResult(1)(m("stasi"))
      intercept[NoSuchElementException] { m("no such key") }
    }

    def `test contains` = {
      val m = Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8)
      assert(m contains "kgb")
      assert(!(m contains "no such key"))
    }

    def `get return optional` = {
      val m = Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8)

      val result1 = m.get("stasi")
      assert(result1.isDefined)
      assert(result1.get == 1)

      val result2 = m.get("no such key")
      assert(result2.isEmpty)
    }

    /*
     * default passed into getOrElse is "call-by-name"
     * which is a lazy-evaluated function
     * other than some concrete value
     */
    def `test getOrElse` = {
      val m = Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8)

      val result1 = m.getOrElse("gru", -1)
      assertResult(8)(result1)

      val result2 = m.getOrElse("no such key", -1)
      assertResult(-1)(result2)
    }

    def `test add` = {

      val m = Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8)

      // update one, and add two new ones
      val newmap = m + ("stasi" -> 100, "cia" -> 4, "fbi" -> 6)

      // check
      assert(newmap.size == 5)
      val expected = Seq("stasi" -> 100, "kgb" -> 9, "gru" -> 8, "cia" -> 4, "fbi" -> 6)
      for ((k, v) <- expected) {
        assert(newmap(k) == v)
      } //for
    }

    def `remove one by one with -` = {
      val m = Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8, "mi5" -> 5, "mi6" -> 6)

      var newmap = m - "stasi"
      newmap = newmap - "no such key" // remove a non-existing key doesn't matter
      newmap = newmap - ("gru", "mi5", "mi6") // remove multiple keys

      assert(newmap.size == 1)
      assert(newmap("kgb") == 9)
    }

    def `remove a set with --` = {
      val m = Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8, "mi5" -> 5, "mi6" -> 6)
      val newmap = m -- Set("gru", "mi5", "mi6") // remove multiple keys
      assertResult(Map("stasi" -> 1, "kgb" -> 9))(newmap)
    }

    def `test iterate` = {
      val m = Map("stasi" -> 1, "kgb" -> 9, "gru" -> 8)

      val txts = for ((k, v) <- m) yield s"$k-$v"
      assert(txts.isInstanceOf[List[String]])
      assert(txts == Seq("stasi-1", "kgb-9", "gru-8"))

      assert(m.keySet.isInstanceOf[Set[String]])
      // m.values is a MapLike type
      assert(m.values.toSeq == Seq(1, 9, 8))
    }

    def `sorted map` = {
      val m = collection.immutable.SortedMap("stasi" -> 1, "kgb" -> 9, "gru" -> 8)
      val keys = m.keySet.toSeq
      assert(keys == Seq("gru", "kgb", "stasi"))
    }

  } // object

}