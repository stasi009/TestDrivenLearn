package demo

import io.Source
import java.io.PrintWriter

object FileIoDemo extends App {

  def demoReadText() = {
    // val source = Source.fromFile("resources/PythonZen.txt")
    val source = Source.fromFile("resources/Python箴言.txt", "utf-8")

    // source can be used as Iterator[Char]
    // lniterator is Iterator[String], and different from Python, the ending '\n' has been trimed
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
  
  def demoWriteText() = {
    // under Mac, by default, it can automatically written as UTF-8
    val fout = new PrintWriter("resources/chinese_texts.txt")
    val texts = Array("我叫赵小胖","我在竹间","我去过重庆")
    for (txt <- texts) {
      fout.println(txt)
    }
    fout.close()
  }

  override def main(args: Array[String]) = {
    // demoReadText()
    // demoIteratorDisposeProblem()
    demoWriteText()
  } //def

}