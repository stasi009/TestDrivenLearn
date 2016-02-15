
library(lubridate)

demo.unix2datetime <- function() {
  seconds = 1363740309
  
  dt <- as.POSIXct(seconds,origin="1970-01-01")
  print(dt)
  
  dt <- as.POSIXct(seconds,origin="1970-01-01",tz="GMT")
  print(dt)
}
