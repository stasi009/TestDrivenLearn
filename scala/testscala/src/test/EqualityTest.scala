package test

import org.scalatest.Spec

sealed class EqualityTest extends Spec {

  object `basic demo` {

    def `demo eq and ==` = {
      val original = "hello scala"
      val interned = "hello scala"
      val newinstance = new String("hello scala")

      // ----------- eq checks identity equality
      assert(original eq interned)
      assert(!(original eq newinstance))

      // ----------- == is based on "equals"
      // by default, "equals" also performs identity equality
      // however, "equals" can be overriden to provide "value-based equality" checking
      assert(original == interned)
      assert(original == newinstance)
    } //def
  } //object
  
  object `user-defined equals` {
    
    sealed class DefEqualItem(val n: Int) 
    
    sealed class UserdefEqualItem(val n:Int) {
      // final prevent this method to be overriden any more
      final override def equals(other: Any) = {
        if (other.isInstanceOf[UserdefEqualItem]) {
          val otherItem = other.asInstanceOf[UserdefEqualItem]
          n == otherItem.n
        }
        else
          false
      }
      
      final override def hashCode = n
      
    }//class
    
    def `default == checks identity` = {
      val original = new DefEqualItem(9)
      val sameRef = original
      val newInstance = new DefEqualItem(9)
      
      assert(original == sameRef)
      assert(original != newInstance)
    }// def
    
    def `override equals to provide value-based equality check` = {
      val original = new UserdefEqualItem(9)
      val newInstance = new UserdefEqualItem(9)
      
      // not the same instance
      assert(! (original eq newInstance) )
      // value-based equality check
      assert(original == newInstance)
    }//def
    
    /*
     * if we override "equals", not only 
     * for two single instance, we can have content-based equality check
     * also, for two collections, we will also have content-based equality check automatically
     */
    def `overriden equals extended to collections` = {
      val numbers = Seq(1,9,8,6,4)
      val sq1 = numbers map {new UserdefEqualItem(_)}
      val sq2 = numbers map {new UserdefEqualItem(_)}
      assert(! (sq1 eq sq2) )
      assert(sq1 == sq2)
    }//def
    
  }//object
}












