package demo

object ConsoleIoDemo extends App {

  def demoPrintf() = {
    val name = "stasi"
    val score = 99.9599
    printf("name=%s,score=%3.2f", name, score)
  }

  // readXXX have been deprecated
  def demoRead() = {
    val name = readLine("please input your name: ")

    println("please input your score: ")
    val score = readFloat()

    printf("name=%s,score=%3.2f", name, score)
  }

  override def main(args: Array[String]) = {
    // demoPrintf()
    demoRead()
  }

}