
library(RUnit)

############## index and slice vector ##############
test.index1 <- function() {
  x <- (1:5)^2
  expected <- c(1,9,25)
  
  # pay attention that R starts its index from 1 not 0
  checkEqualsNumeric( expected, x[c(1,3,5)] )
  
  # in R, negative index doesn't mean counting backwards
  # but means "exclude"
  checkEqualsNumeric(expected,x[c(-2,-4)])
  checkEqualsNumeric(expected,x[c(-4,-2)])
  
  # use boolean vector
  checkEqualsNumeric(expected,x[c(TRUE,FALSE,TRUE,FALSE,TRUE)])
  
  # index by names
  names(x) <- c("one", "four", "nine", "sixteen", "twenty five")
  checkEqualsNumeric(expected,x[c("one", "nine", "twenty five")],checkNames=FALSE)
}

test.index2 <- function() {
  a <- c("x","y","z")
  checkIdentical( c("x","y","z","z","y","x"),a[c(1,2,3,3,2,1)] )
}

test.index.outofrange <- function() {
  x <- 1:3
  # a out-of-range index won't cause error, just return NA
  checkTrue( is.na(x[100]) )
  
  # get a "out of range" index return na, but won't change the underlying vector
  checkIdentical(c(1L,2L,3L),x)
  
  # set a "out of range" index will enlarge the vector, 
  # and fill the missing elements as NA
  x[6] <- 9L
  checkIdentical(c(1L,2L,3L,NA,NA,9L),x)
}

test.which <- function() {
  # which returns the indices
  x <- c(4,9,8,99,36)
  checkEqualsNumeric(c(1,3,5), which(x%%2==0))
}

test.slice.copy <- function() {
  whole_vector <- 1:6
  sub_vector <- whole_vector[2:4]
  checkEquals(c(2,3,4),sub_vector)
  
  sub_vector[2] = -100
  checkEquals(c(2,-100,4),sub_vector)
  
  # sliced vector is a copy
  # changes on the sliced vector will not affect the original vector
  checkIdentical(1:6,whole_vector)
}

############## seq ##############
test.seq.int <- function() {
  checkEquals(  c(3,5),seq.int(3,6,2) )
  # return float numbers even all starting point and ending point are all whole numbers
  checkTrue( is.double(seq.int(3,6,2)) )
  
  checkEquals( c(1,0.7),seq.int(1, 0.5, -0.3) )
  checkEquals( c(1,0.7),seq(1, 0.5, -0.3) )
}

test.seq.by <- function() {
  checkEquals(c( 1.000000,4.141593,7.283185),seq(1, 9, by = pi),tolerance=1e-4)
}

test.seq.lengthout <- function() {
  checkEquals( c(0,0.25,0.5,0.75,1),seq(0, 1, length.out = 5) )
  checkEquals( c(0.0000000,0.3333333,0.6666667,1.0000000),seq(0, 1, length.out = 4),tolerance=1e-4 )
}

test.seq.along <- function() {
  txts <- c("stasi","cheka","kgb")
  checkIdentical( 1:3,seq_along(txts) )# not care the conent, just numbers
}

############## create ##############
test.create.by.constructor <- function() {
  # two ways to create by constructor
  
  # 1. use general "vector"
  checkEquals( c(0,0,0), vector("numeric",3) )
  checkIdentical( c("",""), vector("character",2) )
  checkIdentical( c(FALSE,FALSE), vector("logical",2) )
  
  # 1. use specific constructor
  checkEquals( c(0,0,0,0), numeric(4) )
  checkIdentical( c("",""), character(2) )
  checkIdentical( c(FALSE,FALSE), logical(2) )
}

test.concatenate <- function() {
  # same type
  checkIdentical(c(1L,2L,3L,4L,8L,9L,10L), c(1:4,8:10))
  
  # different types, need cast 
  n1 <- 1:4
  s1 <- c("a","b")
  # numbers are cast into strings, and then concatenate
  checkIdentical( c("1","2","3","4","a","b"),c(n1,s1) )
}

test.create.by.colon <- function() {
  # ascending order
  checkIdentical( c(1L,2L,3L), 1:3,"integer in ascending order")
  
  # descending order
  checkIdentical( c(3L,2L,1L), 3:1,"integer in ascending order")
  
  # it can accept float numbers
  # however, the interval is always one
  checkEquals( c(3.14,4.14,5.14,6.14),3.14:7)
  
  a <- 1:4
  b <- 2:5
  checkIdentical( c(3L,5L,7L,9L),a+b)
}

test.create.by.c <- function() {
  # first it demonstrate that vector can store different types
  v1 <- c(99L,3.14,"cheka")
  v2 <- c(1:4,v1)
  
  # second, it demonstrate that when nest a vector
  # into another vector, it won't keep its structure
  # but flatten all nested one into one huge vector
  checkIdentical(7L,length(v2))
  
  # another test
  v3 <- c(1:3,18:15,-3:-1)
  checkIdentical(c(1L,2L,3L,18L,17L,16L,15L,-3L,-2L,-1L),v3)
}

############## others ##############
test.elementwise.math <- function() {
  v1 <- seq(2,8,3)
  v2 <- 1:5
  checkEqualsNumeric( c(3,7,11,6,10),v1+v2 )
}

test.sametype.feature <- function() {
  # vector can only store same type
  # if different types are passed in
  # a silent conversion will happen
  v1 <- c(1L,1)
  checkTrue(!is.integer(v1))
  checkTrue(is.double(v1))# convert both to float numbers
  
  v2 <- c(1L,1,"stasi")
  checkTrue(is.character(v2)) # all convert to strings
  checkIdentical(c("1","1","stasi"),v2)
}

test.c.flatten.effect <- function() {
  # "c" will not preserve nested structure
  # but flatten everything into one huge vector
  # all elements concatenated into single vector
  v3 <- c(1:3,18:16,-3:-1)
  checkIdentical(c(1L,2L,3L,18L,17L,16L,-3L,-2L,-1L),v3)
}

test.length <- function() {
  # get 
  checkEqualsNumeric(4,length(1:4))
  checkEqualsNumeric(2,length(c(66,88)))
  
  # set a shorter length will truncate
  # set a longer length will insert NA at the end
  x <- 1:4
  length(x) <- 6
  checkIdentical(c(1L,2L,3L,4L,NA,NA),x)
  
  length(x) <- 2
  checkIdentical(c(1L,2L),x)
}

# shorter vector will be recyled to match the longer one
test.recycle <- function() {
  a <- 1:2
  b <- 1:4
  checkIdentical( as.integer( c(2,4,4,6)),a+b)
  checkIdentical( as.integer( c(0,0,-2,-2)),a-b)
  
  c <- 3:5
  checkIdentical( as.integer( c(4,6,6)),a+c)
  checkIdentical( as.integer( c(4,6,8,7)),b+c)
  
  checkIdentical(c(2L,4L,6L,8L,10L,7L,9L),1:5 + 1:7)
}

test.names <- function() {
  # set name during initialization
  fruits <- c(apple = 1,banana = 2, "kiwi fruit" = 3, 4)
  checkEqualsNumeric(3,fruits["kiwi fruit"])
  
  checkIdentical( c("apple","banana","kiwi fruit",""),names(fruits) )
  
  # set names explicitly
  v <- 1:4
  names(v) <- c("a","b","c","d")
  checkEqualsNumeric(4,v["d"])
  checkEquals(4,v["d"],checkNames=FALSE)
}

test.in <- function() {
  checkIdentical(c(TRUE,FALSE,TRUE,FALSE,TRUE,FALSE),1:6 %in% c(1,3,5,9))
}

test.dot.product <- function() {
  v1 <- 1:3
  v2 <- 4:6
  
  inner_product <- v1 %*% v2
  checkIdentical("matrix",class(inner_product))
  checkIdentical(c(1L,1L),dim(inner_product))
  
  scalar <- drop(inner_product)
  checkTrue(32 == scalar)
}

test.match <- function() {
  tomatch <- c(3, 1, 2, 2, 2, 3, 3, 4, 4, 3)
  matchagainst <- c(4,3,5)
  
  actual <- match(tomatch,matchagainst)
  # The function returns a vector of   the same length as the first argument 
  # in which the values are the index of entries in that   vector that match 
  # some value in the second vector.
  expected <- c(2L, NA, NA, NA, NA, 2L, 2L, 1L, 1L, 2L)
  
  checkIdentical(expected,actual)
}



