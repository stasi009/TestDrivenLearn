package demo

object AppObjectDemo extends App {
  // place the codes for "main program" inside the primary constructor

  if (args.length > 0)
    println("hello, " + args(0))
  else
    println("hello scala from stasi")

}