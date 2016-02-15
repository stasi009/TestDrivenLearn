
library(RUnit)

test.Position.Find <- function() {
  v <- c(1,3,4,6,8)
  # Position returns the first index where element satisfies the condition
  checkIdentical(3L, Position(function(x) x%%2==0,v))
  # Find returns the first element itself which satisfies the condition
  checkIdentical(4, Find(function(x) x%%2==0,v))
}

test.Reduce1 <- function() {
  checkIdentical(10L, Reduce("+",1:4))
  checkIdentical(24L, Reduce("*",1:4))
  checkIdentical(72, Reduce("*",1:4,init=3))
  checkIdentical(8, Reduce(function(x,y)ifelse(x >=y,x,y),c(1,4,6,8,3)))
}

test.Reduce.Accumulate <- function() {
  checkIdentical(c(1L,3L,6L,10L), Reduce("+",1:4,accumulate=TRUE))
}

test.Filter <- function() {
  x <- c(5, 8, 1, 4, 2)
  checkIdentical( c(5,1),Filter(function(y) y%%2==1,x) )
}
