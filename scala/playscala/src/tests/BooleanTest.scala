package tests

import org.scalatest.Spec

sealed class BooleanTest extends Spec {
  
  object `basic operation` {
    def `how to compose` = {
      assert(true && true)
      assert(true || false)
      assert(!false)
    }//def
  }//object

}