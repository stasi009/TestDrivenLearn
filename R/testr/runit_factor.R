
library(RUnit)
library(plyr)

demo.cut.table <- function() {
  numbers <- rnorm(100)
  intervals <- seq(-5,5,by=0.5)
  table(cut(numbers,intervals))
}

demo.cut <- function() {
  numbers <- runif(100,0,1000)
  breaks <- c(0, 25, 60,200,Inf)
  slots <- cut(numbers,breaks,c("tiny","small","middle","large"))
  
  assert_is_factor(slots)
  table(slots)
}

test.sample1 <- function() {
  # by default, factor using alphabetic order
  # so female=1, and male=2
  gender <- factor(c("male", "female", "female", "male", "female"))
  
  # levels are sorted according to alphabetic order
  checkIdentical( c("female","male"),levels(gender) )
  
  checkEquals(5,length(gender))
  checkEquals(2,nlevels(gender))
  
  checkIdentical( c(2L,1L,1L,2L,1L),as.integer(gender) )
  checkIdentical( c("male", "female", "female", "male", "female"),as.character(gender) )
}

test.ordered <- function() {
  status <- c("Excellent","Poor", "Improved", "Poor")
  checkTrue(is.character(status))
  
  status <- factor(status,
                   order=TRUE,
                   levels = c("Poor","Improved","Excellent"))
  checkIdentical( c(3L,1L,2L,1L),as.integer( status) )
}

test.revalue <- function() {
  y <- factor(c("a", "b", "c", "a"))
  checkIdentical(c(1L,2L,3L,1L),as.integer(y))
  
  newy <- revalue(y,c(a="Alpha",b="Beta",c="Charle"))
  checkIdentical(c(1L,2L,3L,1L),as.integer(newy))
  
  checkIdentical( c("Alpha","Beta","Charle"),levels(newy) )
}

test.mapvalues <- function() {
  y <- factor(c("a", "b", "c", "a"))
  checkIdentical(c(1L,2L,3L,1L),as.integer(y))
  
  newy <- mapvalues(y,c("a","b","c"),c("Alpha","Beta","Charle"))
  checkIdentical(c(1L,2L,3L,1L),as.integer(newy))
  
  checkIdentical( c("Alpha","Beta","Charle"),levels(newy) )
}
