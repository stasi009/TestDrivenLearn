
####################### helper methods #######################
hypotenuse <- function(x,y) {
	sqrt(x^2+y^2)
}

library(RUnit)

####################### test methods #######################
test.sample1 <- function() {
	expected <- 5
	actual <- hypotenuse(3,4)
	checkEqualsNumeric(expected,actual)
}

test.checkIdentical <- function() {
  checkIdentical(c(4L,5L,6L),4:6)
  checkIdentical(9,3+6)
  checkIdentical("stasi 9",paste("stasi",9))
}

test.checkEqualsNumeric <- function() {
  # test single numeric
  checkEqualsNumeric(5,5.001,tolerance=0.01)
  
  # test vectors
  vec1 <- 1:10
  vec2 <- vec1 + 0.001
  checkEqualsNumeric(vec1,vec2,tolerance=0.01) 
}

test.checkEqualsNumeric.ignore.structure <- function(){
  m1 <- matrix(1:12,nrow=4)
  m2 <- matrix(1:12,nrow=6)
  
  # checkEqual will compare the structure
  checkEquals(m1,m2,check.attributes=FALSE)
  
  # internally, checkEqualsNumeric will call "as.vector"
  # which will ignore the structure and names
  # and only compare the numbers
  checkEqualsNumeric(m1,m2)
}

test.checkTrue <- function() {
  checkTrue(1<2,"this is test message")
}

test.checkException <- function() {
  fool <- function(x) {
    if(x)
    {
      stop("stop conditions signaled")
    }
    return()
  }
  checkException( fool(TRUE) )
}
