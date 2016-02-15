
library(RUnit)

test.mean <- function() {
  a <- 1:5
  checkEqualsNumeric(3, mean(a))
}

test.median <- function() {
  a <- 1:6
  checkEqualsNumeric(3.5, median(a))
}

test.sum <- function() {
  # pass in a vector
  a <- 1:5
  checkEqualsNumeric(15, sum(a))
  
  # can pass in just multiple numbers
  checkEqualsNumeric( 18,sum(1,6,9,2))
}

test.sum.bool <- function() {
  a <- c(FALSE,FALSE,TRUE,FALSE,TRUE,FALSE)
  checkIdentical(2L,sum(a))
}

test.pmax.pmin <- function() {
  v1 <- c(46,60,78,27,11)
  v2 <- c(85,4,53,80,54)
  checkIdentical( c(85,60,78,80,54),pmax(v1,v2) )
  checkIdentical( c(46,4,53,27,11),pmin(v1,v2) )
}

test.cum <- function() {
  v <- c(24, 44, 49, 13, 38)
  checkIdentical( c(24, 68, 117, 130, 168),cumsum(v) )
}