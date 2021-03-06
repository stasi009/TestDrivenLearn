

package test

import org.scalatest.Suite
import org.scalatest.BeforeAndAfter

class ExampleTestSuite extends Suite with BeforeAndAfter {
  
  // java's list can only accept Integer, not scala.Int
  var _list : java.util.ArrayList[Integer] = _
  
  before {
    _list = new java.util.ArrayList[Integer]
  }
  
  after {
    _list = null
  }
  
  def testListEmpty() = {
    assert(_list.size() === 0)
  }
  
  def testListAdd() = {
    _list.add(1)
    _list add 9
    assert(_list.size() === 2,"unexpected size of list")
    assertResult(2, "unexpected size of list"){_list.size()}
  }
  
  def testErrorWhenGetEmptyList() = {
    intercept[IndexOutOfBoundsException]{_list.get(0)}
  }

}