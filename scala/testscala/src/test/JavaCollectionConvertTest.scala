package test

import org.scalatest.Spec
import collection.JavaConversions._
import collection.mutable
import java.util.{List=>JList}

sealed class JavaCollectionConvertTest extends Spec {
  
  object `test conversion` {
    
    def `arraybuffer to java list` = {
      val ab = mutable.ArrayBuffer(1,2,3)
      
      /*
       * these conversion work by setting up a wrapper object that forwards all operations to 
       * the underlying collection object. So collections are not copied when converting between Java and Scala
       */
      val jIntList: JList[Int] = ab
      
      /*
       * since there is an explicit conversion
       * so here, values are copied
       */
      val jIntegerList: JList[Integer] = ab map {x=>x:java.lang.Integer}
      
      // changes will reflect on underlying collection
      jIntList.add(100)
      jIntegerList.add(-1)
      
      assert(ab sameElements Array(1,2,3,100))
      assert(jIntList.size == 4)
      
      assert(jIntegerList.size == 4) 
      assert(jIntegerList.toSeq == Seq(1,2,3,-1))
    }//def
    
  }//object

}//class