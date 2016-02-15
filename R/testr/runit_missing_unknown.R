
library(RUnit)

test.na <- function() {
  l <- c(10,20,30,NA,40)
  
  # NA is included into calculation by default
  checkTrue(is.na(mean(l)))
  
  # exclude NA explicitly by setting rm.na=TRUE
  # remove both from the numerator and denominator
  checkEqualsNumeric( 25,mean(l,na.rm=TRUE) )
  
  # NA also occupy memory
  checkIdentical(5L,length(l))
  checkTrue( is.na(l[4]) )
  
  # is.na is vectorized function, which will check each element in the collection
  checkIdentical( c(FALSE,FALSE,FALSE,TRUE,FALSE),is.na(l) )
}

test.null <- function() {
  l <- c(10,20,30,NULL,40)
  
  checkIdentical(4L,length(l),"NULL not occupy memory")
  checkEqualsNumeric(40,l[4])
  
  checkEqualsNumeric( 25,mean(l),"exclude automatically" )
  
  checkIdentical(c("x=10","x=20","x=30","x=40"),paste("x=",l,sep=""))
  
  checkIdentical( FALSE,is.null(l),"is.null is not vectorized, it treat its parameter as a whole")
}

test.complete.cases <- function() {
  x <- c(1,NA,2,3,NA)
  # complete.cases return TRUE when the element is NOT missing
  checkIdentical(c(TRUE,FALSE,TRUE,TRUE,FALSE), complete.cases(x))
  
  y <- c(NA,3,4,5,6)
  dt <- data.frame(x=x,y=y)
  
  actual <- complete.cases(dt)
  expected <- (complete.cases(x)) & (complete.cases(y))
  checkIdentical(expected,actual)
}

test.na.omit <- function() {
  dt <- data.frame(x=c(1,NA,2,3,NA),
                   y=c(NA,3,4,5,6))
  na.free <- na.omit(dt)
  
  checkEqualsNumeric(c(2,3),na.free$x)
  checkEqualsNumeric(c(4,5),na.free$y)
}

test.na.fail <- function() {
  x <- c(1,NA,2,3,NA)
  checkException( na.fail(x) )# raise exception when there is NA existing
  
  # OK when there is no missing values
  na.fail(c(1,2,3))
}















