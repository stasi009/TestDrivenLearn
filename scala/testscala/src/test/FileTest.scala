package test

import org.scalatest.Spec
import java.io.File

sealed class FileTest extends Spec {

  object `test path` {

    def `test parent and base name` = {
      val filename = "C:\\dev\\project\\file.txt"
      val file = new File(filename)
      assert(file.getParent == "C:\\dev\\project")
      assert(file.getName == "file.txt")
    } //def

  } //object

}//class