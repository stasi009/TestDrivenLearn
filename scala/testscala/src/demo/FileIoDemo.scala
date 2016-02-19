package demo

import io.Source

object FileIoDemo extends App {

  def demoReadText() = {
    val source = Source.fromFile("resources/PythonZen.txt")
    val lniterator = source.getLines
    lniterator.zipWithIndex.foreach { case (ln, index) => println(s"[$index]:$ln") }
    source.close()
  } //def

  /*
   * below codes demonstrate a problem:
   * the underlying source must be kept open during the whole period that the iterator is used
   * if the iterator is still used, but the underlying source is closed
   * there will be an error
   * 
   * this is different from seq{} expression in F#
   * where the "use in seq" will guarantee that the source will be closed only 
   * after all items in the iterable/iterator are consumed
   */
  def demoIteratorDisposeProblem() = {

    def iteratorFromFile(filename: String) = {
      val source = Source.fromFile(filename)
      try {
        source.getLines
      } finally { source.close() }
    }

    val iterator = iteratorFromFile("resources/PythonZen.txt")
    iterator.zipWithIndex.foreach { case (ln, index) => println(s"[$index]:$ln") }
  }

  override def main(args: Array[String]) = {
    // demoReadText()
    demoIteratorDisposeProblem()
  } //def

}