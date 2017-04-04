package demo

object ConsoleIoDemo extends App {
  
  /**
   * this demo function shows 
   * 1. println can automatically add '\n' at the end
   * 2. Scala supports Unicode string automatically
   */
  def demoPrintln() = {
    val name = "赵小胖"
    val company = "竹间智能"
    println("我是%s, 来自%s, 请多多关照。" format (name,company))
  }

  /**
   * printf: C-style print
   */
  def demoPrintf() = {
    val name = "stasi"
    val score = 99.9599
    // you have include '\n' yourself
    printf("name=%s,score=%3.2f\n", name, score)
    printf("score=%3.2f,name=%s\n", score, name)
  }

  // readXXX have been deprecated
  def demoRead() = {
    // only readLine can have prompt string
    val name = readLine("please input your name: ")

    println("please input your score: ")
    val score = readFloat()

    printf("name=%s,score=%3.2f", name, score)
  }

  override def main(args: Array[String]) = {
    demoPrintln()
    // demoPrintf()
    // demoRead()
  }

}