package demo

object OtherDemo extends App {

  def demoForeach() = {
    val numbers = Seq("a", "b", "c")
    // the parameter is a tuple, not two parameters
    ((1 to 3) zip numbers) foreach { t => println(s"${t._1}: ${t._2}") }
  }

  override def main(args: Array[String]) = {
    demoForeach()
  } //def main

}