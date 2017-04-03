package demo

object AppObjectDemo extends App {
  if (args.length > 0)
    println("hello, " + args(0))
  else
    println("hello scala from stasi")
}