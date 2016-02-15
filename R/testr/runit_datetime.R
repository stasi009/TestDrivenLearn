
library(RUnit)
library(lubridate)

test.parse <- function() {
  txt <- "20:17:40 20/07/2013"
  dt <- strptime(txt,"%H:%M:%S %d/%m/%Y")
  checkIdentical(113L, dt$year)# year returns "years since 1900"
  checkIdentical(6L, dt$mon)#0–11: months after the first of the year.
  checkIdentical(20L, dt$mday)
  checkIdentical(20L, dt$hour)
  checkIdentical(17L, dt$min)
  checkIdentical(40, dt$sec)
}

test.lubridate.parse <- function() {
  strtime <- "14-04-14 15:28:25"
  timect <- ymd_hms(strtime)
  checkTrue( is.POSIXct(timect) )
  
  timelt <- as.POSIXlt(timect)
  checkIdentical(114L,timelt$year)
  checkIdentical(3L,timelt$mon)
  checkIdentical(14L,timelt$mday)
  checkIdentical(15L,timelt$hour)
  checkIdentical(28L,timelt$min)
  checkIdentical(25,timelt$sec)
}
