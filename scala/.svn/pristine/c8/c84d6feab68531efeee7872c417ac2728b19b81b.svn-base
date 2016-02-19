package test

import org.scalatest.Spec
import collection.immutable.Range
import collection.immutable.IndexedSeq
import common.Utility

class RangeTest extends Spec {
  
  object `simple test` {
    
    def `range returned by to` = {
      val range = 1 to 10
      assert(range.isInstanceOf[Range])
      assert(range.isInstanceOf[IndexedSeq[Int]])
      
      assert(range.length == 10)
      assert(range.contains(1))
      assert(range.contains(10))//include both ends
      assert(!range.contains(99))
    }
    
    def `range returned by until` = {
      // by using until, it returns [lowbound,highbound)
      val range = 1 until 10
      assert(range.isInstanceOf[Range])
      
      assert(range.length == 9)
      assert(range.contains(1))
      assert(! range.contains(10))//not include the high boundary
    }
    
    def `by intervals` = {
      val testtuples = Array(
        (0.0 to 1.0 by 0.25, Array(0.0, 0.25, 0.5, 0.75, 1.0)),
        (0.0 to 1.0 by 1.0 / 3.0, Array(0.0, 0.3333333333333333, 0.6666666666666666, 1.0)),
        (0.0 to 1.0 by 0.4, Array(0.0, 0.4, 0.8)),
        (0.0 to 1.0 by 0.33333333, Array(0.0, 0.33333333, 0.66666666, 0.99999999)))

      for (t <- testtuples) {
        assert(Utility.seqAlmostEqual(t._1, t._2))
      }
    }
    
    def `every n elements` = {
      val range = 0 to (10,2)
      assertResult(Seq(0,2,4,6,8,10))(range.toSeq)
    }
    
  }// simple test

}