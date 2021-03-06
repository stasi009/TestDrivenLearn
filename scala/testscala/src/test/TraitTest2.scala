package test

import org.scalatest.Spec

sealed class TraitTest2 extends Spec {

  case class DataItem(id: Int)

  sealed class DbEngine {
    def find(id: Int) = DataItem(id)

    def add(id: Int) = throw new NotImplementedError
    def remove(id: Int) = throw new NotImplementedError

    def createTable(name: String) = throw new NotImplementedError
    def dropTable(name: String) = throw new NotImplementedError
  }

  trait ReadOnly {
    // ------------------ abstract members
    val underlying: DbEngine // abstract member field waiting to be override

    // ------------------ concrete implementation
    def find(id: Int) = underlying find id
  }

  trait Updateable extends ReadOnly {
    def +=(id: Int) = underlying add id
    def -=(id: Int) = underlying remove id
  }

  trait Administrator extends ReadOnly {
    def createTable(name: String) = underlying createTable name
    def dropTable(name: String) = underlying dropTable name
  }
  
  class Reader extends ReadOnly {val underlying = new DbEngine}
  
  trait Cacheable extends ReadOnly {
    private val _history = collection.mutable.Map[Int,DataItem]()
    override def find(id: Int) = _history.getOrElseUpdate(id, super.find(id))
  }

  object `DAO based on Trait` {
    
    def getReader = new Reader with Cacheable
    def getUpdater = new Reader with Updateable with Cacheable
    def getAdministrator = new Reader with Administrator with Cacheable
    
    def `test cacheable` = {
      val reader = getReader
      
      val item1 = reader.find(1)
      val item2 = reader.find(1)
      
      assert(item1 eq item2)
    }

  } //object

}