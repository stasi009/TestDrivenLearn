
library(RUnit)

test.vector.immutable <- function() {
  original <- 1:4
  copy <- original 
  
  original[2] <- -99
  checkEquals(c(1,-99,3,4),original)
  
  # copy is an isolated copy, NOT just copy the reference
  # changes on the original won't affect copy
  checkEquals(c(1,2,3,4),copy)
  
  # same for the reverse direction
  # changes on the copy won't affect the original
  copy[1] <- 100
  checkEquals(c(100,2,3,4),copy)
  checkEquals(c(1,-99,3,4),original)
}

test.list.immutable <- function() {
  original <- list(a=1)
  copy <- original
  checkIdentical(1,copy$a)
  
  original$a <- 99
  checkIdentical(99,original$a)
  
  checkIdentical(1,copy$a)# NOT CHANGED
}

test.pass.argument <- function() {
  original <- list(a=1)
  fool <- function(p){p$a <- 100; p}
  copy <- fool(original)
  
  checkIdentical(100,copy$a)
  checkIdentical(1,original$a)
}