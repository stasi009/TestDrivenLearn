
library(RUnit)

test.vectorized.equal <- function() {
  a <- c(3, 4 - 1, 1 + 1 + 1)
  checkIdentical( rep(TRUE,3),a == 3)
  checkIdentical( rep(TRUE,3),a == c(3,3,3))
  
  # I think, if it is a whole number
  # internally, R will treat it as integer, although not identical
  b <- c(1,2,3)
  checkIdentical("numeric",class(b))
  
  c <- 1:3
  checkIdentical("integer",class(c))
  
  checkTrue(!identical(b,c))
  checkIdentical(rep(TRUE,3),b==c)
}

test.identical1 <- function() {
  # pay attention that, we have to use "as.integer" 
  # otherwise, c(1,2,3,4) will be treated as double
  # so identical will return false
  a <- c(1,2,3,4)
  b <- 1:4
  a.int <- as.integer(a)
  
  checkTrue(! identical(a,b))
  checkTrue(identical(a.int,b),"identical returns a single logic value")
  
  # below tests demonstrate that, although a is NOT identical to b
  # however, that is mainly because a is float, while b is integer
  # but underneath, numerically, they are equal
  checkIdentical(c(TRUE,TRUE,TRUE,TRUE), a == b,"== performs element-wise comparing")
  checkIdentical(c(TRUE,TRUE,TRUE,TRUE), a.int == b,"== performs element-wise comparing")
}

test.all.equal <- function() {
  delta = 0.001
  a <- 1:10
  b <- a+delta
  checkTrue( isTRUE( all.equal(a,b,tolerance=10*delta)) )
  checkTrue( !isTRUE( all.equal(a,b,tolerance=0.1 * delta)) )
}

test.all.equal.names <- function() {
  x <- c(a=1,b=2,c=3)
  y <- 1:3
  checkTrue( ! isTRUE(all.equal(x,y)) )# not equal, because one has name, while the other doesn't have name\
  checkTrue ( all.equal(x,y,check.names=FALSE),"equal without names")
}

test.all.equal.structure <- function() {
  wholematrix <- matrix(1:12,nrow=4,
                        dimnames = list(paste("row",1:4,sep=""),
                                        paste("col",1:3,sep="")))
  submatrix <- wholematrix[c("row2","row3"),c(-2)]
  
  expected <- matrix(c(2,3,10,11),nrow=2)
  
  checkTrue(!isTRUE(all.equal(expected,submatrix)) )
  checkTrue(isTRUE(all.equal(expected,submatrix,check.attributes=FALSE) ))
}
