
package test

import org.scalatest.Spec
import org.scalatest.BeforeAndAfter
import collection.mutable.ListBuffer
import collection.mutable.ArrayBuffer

class ExampleTestSpec extends Spec with BeforeAndAfter {

  object `Demo Assert` {
    def `should have size 0` {
      assert(Set.empty.size === 0)
    }

    def `should produce NoSuchElementException when head is invoked` {
      intercept[NoSuchElementException] { Set.empty.head }
    }
    
    def `demo assertResult` {
      val abuf = ArrayBuffer(1,2,3)
      assertResult(3,"unexpected size")(abuf.length)
      
      abuf += (8,9)
      assertResult(5,"size will also work")(abuf.size)
    }
  } // object WhenEmpty

  object `Demo Before After` {

    val builder = new StringBuilder
    val buffer = new ListBuffer[String]

    before {
      builder.append("ScalaTest is ")
    }

    after {
      builder.clear()
      buffer.clear()
    }

    def `should be easy` {
      builder.append("easy!")
      assert(builder.toString === "ScalaTest is easy!")
      assert(buffer.isEmpty)
      buffer += "sweet"
    }

    def `should be fun` {
      builder.append("fun!")
      assert(builder.toString === "ScalaTest is fun!")
      // changes in shared state in one method won't affect another method
      assert(buffer.isEmpty)
    }
  } // demo before and after
}