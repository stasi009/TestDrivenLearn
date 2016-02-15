
library(RUnit)

test.rep.simple <- function() {
  a <- 1:3
  checkIdentical( as.integer(c(1,2,3,1,2,3)),rep(a,2) )
  checkIdentical( as.integer(c(1,1,2,2,3,3)),rep(a,each=2) )
  
  checkIdentical( c(1L,1L,2L,2L,2L,3L,3L,3L,3L), rep(a,times=c(2,3,4)) )
  checkIdentical( c(1L,2L,3L,1L,2L), rep(a,length.out=5) )
}

test.unique <- function() {
  a <- c(1,12,1,2,1,3,2,4)
  checkEquals( c(1,12,2,3,4),unique(a) )
}

test.inline.assignment <- function() {
  x <- {
    a <- 3
    b <- 4
    a*a + b*b
  }
  checkEqualsNumeric(25,x)
}

test.split1 <- function() {
  x <- 1:10
  y <- c(1,2,3,2,1,1,2,2,1,3)
  g <- split(x,y)
  checkIdentical(c(1L,5L,6L,9L), g[[1]])
  checkIdentical(c(2L,4L,7L,8L), g[[2]])
  checkIdentical(c(3L,10L), g[[3]])
}

test.split2 <- function() {
  d <- data.frame(
    name = c("a","b","a","c", "c","b","a"),
    score = 1:7,
    level =  c(4,5,2,1,3,5,8))
  g <- with(d,split(score,name))
  
  expected <- list(a=c(1L,3L,7L),b=c(2L,6L),c=c(4L,5L))
  checkIdentical(expected,g)
}
