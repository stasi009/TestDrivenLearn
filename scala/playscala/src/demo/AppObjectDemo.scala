package demo

/*
 * execute this App by running: scala -cp bin demo.AppObjectDemo
 */
object AppObjectDemo extends App {
  if (args.length > 0)
    println("hello, " + args(0))
  else
    println("hello scala from stasi")
}