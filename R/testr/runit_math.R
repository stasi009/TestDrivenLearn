
library(RUnit)

test.power <- function() {
  # both ways can calculate power of a number
  checkEqualsNumeric(8,2**3)
  checkEqualsNumeric(8,2^3)
}

test.division <- function() {
  # float division
  a <- c(1,3,5,8)
  checkEquals( c(0.3333333,1.0000000,1.6666667,2.6666667),a/3,tolerance=1e-4 )
  
  # integer division
  checkEquals( c(0,1,1,2),a%/%3,tolerance=1e-4 )
  
  # mode
  checkEquals( c(1,0,2,2),a%%3,tolerance=1e-4 )
}