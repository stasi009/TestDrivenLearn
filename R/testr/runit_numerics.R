
library(RUnit)

test.explicit.constructor <- function() {
  checkIdentical(c(0,0,0),numeric(3))
  checkIdentical(c(0L,0L),integer(2))
}

test.float2int <- function() {
  n <- 2.9
  # none of below methods return integer
  # they just return floats which has no decimal parts
  checkIdentical(2,trunc(n))
  checkIdentical(3,ceiling(n))
  checkIdentical(2,floor(n))
  checkIdentical(3,round(n))
  
  # as.integer return an integer
  checkIdentical(2L,as.integer(n))
}
