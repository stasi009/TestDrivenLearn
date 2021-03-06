package demo

object PatternMatchDemo extends App {

  object Student {
    def unapply(msg: String) = {
      val parts = msg.split(",")
      if (parts.length == 3) Some(parts(0), parts(1), parts(2))
      else None
    } //def
  } //extractor object

  // how to write "silent ignore" in pattern match
  def demoIgnore() = {
    val students = Seq(
      "B123456,Justin,Kaohsiung",
      "B98765,Monica,Kaohsiung",
      "xxxxxxxxxxxxxxxxxxxxx",
      "B246819,Bush,Taipei")

    students.foreach(_ match {
      case Student(nb, name, addr) => println(nb + ", " + name + ", " + addr)
      case _ => // silently ignore "not matched"
    })
  }

  object Uppercase {
    // the result type cause implicit conversion
    // which convert Array[Char] to Seq[Char] implicitly
    def unapplySeq(s: String): Option[Seq[Char]] = {
      Some(for (c <- s.toArray if c.isUpper) yield c)
    }
  }

  def demoUnapplySeq() = {
    val Uppercase(u1 @ _*) = "This is Justin"
    u1.foreach(print) // TJ

    println("\n---------------------")

    val Uppercase(u2 @ _*) = "'JL' stands for Justin Lin"
    u2.foreach(print) // JLJL 
  }

  /*
   * optional is like a collection with 0 or 1 elements
   */
  def demoOptionalLikeCollection() = {
    def __fool(o: Option[Int]) = {
      for (n <- o) println(n) // for loop will skip None
    }

    __fool(Some(99))
    __fool(None)
  } //def

  override def main(args: Array[String]) = {
    // demoIgnore()
    // demoUnapplySeq()
    demoOptionalLikeCollection()
  }

}