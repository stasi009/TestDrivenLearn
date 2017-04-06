package tests

import org.scalatest.Spec

sealed class EnumerationTest extends Spec {
  
  object TrafficLightColor extends Enumeration {
    val Red,Yellow,Green = Value
  }// object
  
  /*
   * pay attention that the type of the enumeration is not TrafficLightColor
   * but TrafficeLightColor.Value
   */
  def doWhat(color: TrafficLightColor.Value) = {
    color match {
      case TrafficLightColor.Red => "Stop"
      case TrafficLightColor.Yellow => "Caution"
      case TrafficLightColor.Green => "Go"
    }//match
  }
  
  object `demo enumeration` {
    def `test values` = {
      val allcolors = TrafficLightColor.values map {_.toString}
      assert(allcolors == Set("Red","Yellow","Green"))
    }
    
    def `get enumeration` = {
      // ---------------- get by id
      // id of each enumeration, is one more than its previous, starting from zero
      assert(TrafficLightColor(1) == TrafficLightColor.Yellow  )
      
      // ---------------- get by name
      assert(TrafficLightColor.withName("Red") == TrafficLightColor.Red )
    }
    
    def `pattern match against enumeration` = {
      val colors = Seq(TrafficLightColor.Red ,TrafficLightColor.Green ,TrafficLightColor.Yellow)
      val actions = colors map doWhat // here, "doWhat" method is automatically transformed into function
      assert(actions == Seq("Stop","Go","Caution"))
    }
    
  }//object

}