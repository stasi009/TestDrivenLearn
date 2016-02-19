package test

import org.scalatest.Spec
import java.io.IOException

sealed class ExceptionTest extends Spec {
  
  private val ThrowRuntimeEx = 1
  private val ThrowIllegalArgEx = 2
  private val ThrowIoEx = 3
  
  sealed class Tester {
    private var _runtimeEx = false
    private var _illegalArgEx = false
    private var _ioEx = false
    private var _finally = false
    
    def runtimeExCaught = _runtimeEx
    def illegalArgExCaught = _illegalArgEx
    def ioExCaught = _ioEx
    def finallyInvoked = _finally
    
    def run(choice: Int) = {
      try {
        choice match {
          case `ThrowRuntimeEx` => throw new RuntimeException
          case `ThrowIllegalArgEx` => throw new IllegalArgumentException
          case `ThrowIoEx` => throw new IOException
        }
      }// try 
      catch {
        case ex: IllegalArgumentException => _illegalArgEx = true
        case ex: RuntimeException => _runtimeEx = true
      } //catch
      finally {
        _finally = true
      }//finally
    }//run
  }
  
  object `demo usage` {
    
    def safeDivide(x: Int,y: Int) = {
      try{
        Some(x/y)
      } catch {
        case _: ArithmeticException => None
      }// case
    }// safeDivide
    
    def `demo catch` = {
      assertResult(2)(safeDivide(5, 2).get)
      assert(safeDivide(6, 0).isEmpty)
    }
    
    def `throw sub exception` = {
      val t = new Tester
      t.run(ThrowIllegalArgEx)
      assert(t.illegalArgExCaught)
      assert(!t.runtimeExCaught)
      assert(!t.ioExCaught)
      assert(t.finallyInvoked)
    }

    def `throw super exception` = {
      val t = new Tester
      t.run(ThrowRuntimeEx)
      assert(!t.illegalArgExCaught)
      assert(t.runtimeExCaught)
      assert(!t.ioExCaught)
      assert(t.finallyInvoked)
    }
    
    def `throw MatchError` = {
      val t = new Tester
      t.run(-100000000)// it will throw "MatchError"
      assert(!t.illegalArgExCaught)
      assert(t.runtimeExCaught)// because MatchError belongs to "RuntimeException"
      assert(!t.ioExCaught)
      assert(t.finallyInvoked)
    }

    def `auto propagate up` = {
      val t = new Tester
      intercept[IOException] { t.run(ThrowIoEx) }
      assert(t.finallyInvoked)
    }

    def `catch propagated exception outside` {
      var ioEx = false
      val t = new Tester
      try {
        t.run(ThrowIoEx)
        fail("should never run this line")
      } catch {
        case ex: IOException => ioEx = true
      }
      assert(t.finallyInvoked)
      assert(ioEx)
    }
    
  }

}