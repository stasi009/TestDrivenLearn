package test

import org.scalatest.Spec

sealed class ForLoopTest extends Spec {

  object `demo usage` {

    case class Person(id: Int, name: String)
    val _people = Vector(Person(1, "stasi"), Person(8, "kgb"), Person(9, "cheka"))

    // extractor
    object Student {
      def unapply(msg: String) = {
        val parts = msg.split(",")
        if (parts.length == 3) Some(parts(0).toInt, parts(1), parts(2))
        else None
      } //def
    } //extractor object

    // match failure is siliently ignored
    def `for loop and pattern match 1` = {
      val students = Seq(
        "11,Justin,Kaohsiung",
        "yyyyyyyyyyyyy",
        "98,Monica,Kaohsiung",
        "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
        "66,Bush,Taipei")
      val ids = for (Student(id, name, addr) <- students) yield id
      assert(ids == Seq(11, 98, 66))
    } //def

    /*
     * the sequence yield has the same type as the input
     * this is also proves that: 
     * "for loop" through a concrete collection
     * its result is also a concrete collection, other than some lazy iterators
     */
    def `same type principle` = {
      // input is a vector, then output is also a vector
      val idvector = for (p <- _people) yield p.id
      assert(idvector.isInstanceOf[Vector[Int]])
      assert(Vector(1, 8, 9) == idvector)

      val idarray = for (p <- _people.toArray) yield p.id
      assert(idarray.isInstanceOf[Array[Int]])
      assert(idarray sameElements Array(1, 8, 9))
    }

    /*
     * if there are multiple generators
     * the type of the result is compatible with the first generator
     */
    def `compatible with the first generator` = {
      val a = Array(1, 2)
      val v = Vector(1, 2)

      val resultArray = for (i <- a; j <- v) yield i + j
      assert(resultArray.isInstanceOf[Array[Int]])
      assert(resultArray sameElements Array(2, 3, 3, 4))

      val resultVector = for (i <- v; j <- a) yield i + j
      assert(resultVector.isInstanceOf[Vector[Int]])
      assert(resultVector === Vector(2, 3, 3, 4))
    }

    def `define new variables` = {
      val txts = for {
        i <- 1 to 3
        from = 4 - i
        j <- from to 3
      } yield s"<$i,$j>"
      assertResult(Vector("<1,3>", "<2,2>", "<2,3>", "<3,1>", "<3,2>", "<3,3>"))(txts)
    }

    def `cross multiple lines` = {
      val txts = for {
        p <- _people
        txt = s"${p.name}-${p.id}"
      } yield txt
      assert(Vector("stasi-1", "kgb-8", "cheka-9") == txts)
    }

    def `with guard` = {
      // every generator can have its own guard
      val txts = for {
        i <- 1 to 4
        if i % 2 == 0
        j <- 1 to 4
        if j != i
      } yield s"[$i,$j]"
      assertResult(Vector("[2,1]", "[2,3]", "[2,4]", "[4,1]", "[4,2]", "[4,3]"))(txts)
    }

    def `multiple variables` = {
      val actual = for (i <- 1 to 3; j <- 4 to 6) yield s"[$i,$j]"
      val expected = Vector("[1,4]", "[1,5]", "[1,6]", "[2,4]", "[2,5]", "[2,6]", "[3,4]", "[3,5]", "[3,6]")
      assertResult(expected)(actual)
    }

    def `simple demo` = {
      var sum = 0
      for (i <- Array(9, 8, 6)) {
        sum += i
      }
      assertResult(23)(sum)
    }
  }
}