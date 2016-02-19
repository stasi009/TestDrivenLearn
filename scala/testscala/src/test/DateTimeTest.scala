package test

import org.scalatest.Spec
import java.util.Calendar

sealed class DateTimeTest extends Spec {

  object `test Calender` {

    def `test clear` = {
      val calendar = Calendar.getInstance
      calendar.setTimeInMillis(0)

    } //def

  } //object

}