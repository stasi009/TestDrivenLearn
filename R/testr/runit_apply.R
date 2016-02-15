
library(RUnit)

test.apply.return.scalar <- function() {
  m <- matrix(1:20,nrow=5)
  checkIdentical( c(16L,17L,18L,19L,20L),apply(m,1,max) ) # 1 represents "by row"
  checkIdentical( c(5L,10L,15L,20L),apply(m,2,max) ) # 2 represents "by column"
}

test.apply.return.vector <- function() {
  fool <- function(x) {
    c(min(x),max(x),sum(x))
  }
  
  # each function passed into apply, return a vector
  # vector is also organized into columns into the result matrix
  # each vector (row or column) return its result vector
  # and these vectors are organized by column
  m <- matrix(1:6,nrow=3)
  checkIdentical( matrix(c(1L,4L,5L,2L,5L,7L,3L,6L,9L),nrow=3),apply(m,1,fool) )
  checkIdentical( matrix(c(1L,3L,6L,4L,6L,15L),nrow=3),apply(m,2,fool) )
}

test.lapply.on.list <- function() {
  # "l" stands for "list"
  # so lappy accepts a list (or data.frame)
  # and returns a list (only list, not data.frame even when input is data.frame)
  d <- data.frame(x=1:5, y=6:10)
  # the function will be applied to each vector (each column) within the data.frame
  actual <- lapply(d,function(x) x**2)
  
  expected <- list(
    x=c(1,4,9,16,25),
    y=c(36,49,64,81,100))
  
  checkIdentical(expected,actual)
}

test.lapply.on.vector <- function() {
  v <- 1:5
  actual <- lapply(v,function(x) x*3)
  expected <- as.list( seq(3,15,by=3) )
  checkIdentical(expected,actual)
}

# "v" means return a vector
test.vapply <- function() {
  d <- data.frame(x=1:5, y=6:10)
  checkIdentical( 
    list(x=15L,y=40L),
    lapply(d,sum) )
  
  # the last parameter is a template
  # integer(1) is not "an integer whose value is 1"
  # but a integer vector whose length=1, has only one value which is 0
  # like "[0]"
  # which specify the type of the return value
  checkIdentical(
    c(x=15L,y=40L),
    vapply(d,sum,integer(1)))
}

# "s" means "simplifying"
# it try to simplify the result to an appropriate vector or array if it can
test.sapply.return.scalar <- function() {
  d <- data.frame(x=1:5, y=6:10)
  checkIdentical( c(x=15L,y=40L),sapply(d,sum) )
  
  v <- 1:4
  checkIdentical( c(1L,4L,9L,16L),sapply(v,function(x) x*x) )
}

test.sapply.return.vector <- function() {
  d <- data.frame(x=1:5,y=6:10)
  actual <- sapply(d,function(x) c(min(x),max(x),sum(x)))
  
  expected <- matrix(c(1L,5L,15L,6L,10L,40L),nrow=3)
  colnames(expected) <- c("x","y")
  
  checkIdentical(expected,actual)
}

test.expand.vector.into.dataframe <- function() {
  v <- 1:4
  mat <- sapply(v, function(x) c(x*2,x*x,x**3))
  
  # each element in the original vector
  # returns its result as one column
  # so "mat" is like "feature * samples"
  df <- as.data.frame( t(mat) )
  colnames(df) <- c("double","square","cube")
  
  checkEquals( c(2,4,6,8),df[,"double"] )
}

test.pass.extra.arguments <- function() {
  # the first argument is from the vector
  # the second argument can be passed externally
  fool <- function(x,y) {
    x+y
  }
  
  checkIdentical( c(101,102,103,104), sapply(1:4,fool,100) )
  checkIdentical( c(0,1,2,3), sapply(1:4,fool,y=-1) )
}

test.sapply.on.matrix <- function() {
  m <- matrix(1:6,nrow=3)
  
  # below result is because: when applied to a matrix or an array
  # it will apply the target function to each element one at a time   (moving down columns)
  checkIdentical(c(1L,2L,3L,4L,5L,6L),sapply(m,sum))
  
  # so the correct way is to use "apply" and specify dimension
  # 1 for "by row", 2 for "by column"
  checkIdentical( c(5L,7L,9L),apply(m,1,sum) )
  checkIdentical( c(6L,15L),apply(m,2,sum) )
}

test.mapply <- function() {
  actual <- mapply(paste,c(1, 2, 3, 4, 5),
                   c("a", "b", "c", "d", "e"), 
                   c("A", "B", "C", "D", "E"),MoreArgs=list(sep="-"))
  expected <- c("1-a-A","2-b-B","3-c-C","4-d-D","5-e-E")
  checkIdentical(expected,actual)
}

# Function by is an object-oriented wrapper for tapply applied to data frames.
# example: by(gw.f$gw, IND = gw.f$year, FUN = summary)
test.tapply <- function() {
  names <- c("a","b","a","c", "c","b","a")
  scores <- 1:length(names)
  d <- data.frame(
    names = names,
    scores = scores
    )
  actual <- with(d,tapply(scores,names,mean))
  expected <- c(a=3.666667,b=4,c=4.5)
  checkEqualsNumeric(expected,actual,tolerance=1e-3)
}


