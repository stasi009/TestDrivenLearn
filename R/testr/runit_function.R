
library(RUnit)

test.invoke <- function() {
  # one line code, neglect {}
  fool <- function(x,y,z=5) sprintf("<%3.2f,%3.2f,%3.2f>",x,y,z)
  
  checkIdentical("<3.00,4.00,5.00>",fool(3,4))
  
  # named arguments
  checkIdentical( "<3.14,2.00,5.00>",fool(y=2,x=3.14) )
  
  # override default arguments
  checkIdentical( "<3.14,2.00,1.10>",fool(z=1.1,y=2,x=3.14) )
}

test.defaultargs.as.expression <- function() {
  normalize <- function(x,
                        m=mean(x,na.rm=na.rm),
                        s=sd(x,na.rm=na.rm),
                        na.rm = FALSE){
    (x-m)/s
  }# normalize
  
  checkTrue(all(is.na(normalize(c(1, 3, 6, 10, NA)))))
  
  normed <- normalize(c(1, 3, 6, 10, NA),na.rm=TRUE)
  checkEqualsNumeric( 0, sum(normed,na.rm=TRUE) )
  checkEqualsNumeric( 1,sd(normed,na.rm=TRUE) )
}

test.do.call <- function() {
  # do.call allow passig arguments as a list
  # instead of passing one by one
  fool <- function(x,y) sqrt(x^2+y^2)
  checkEqualsNumeric(5, fool(3,4))
  checkEqualsNumeric(5, do.call(fool,list(3,4)) )
  
  ############################################
  dfr1 <-data.frame(x = 1:5,y =rt(5, 1))
  dfr2 <-data.frame(x = 6:10,y =rf(5, 1, 1))
  dfr3 <-data.frame(x = 11:15,y =rbeta(5, 1, 1))
  
  combined1 <- rbind(dfr1,dfr2,dfr3)
  combined2 <- do.call(rbind,list(dfr1,dfr2,dfr3))
  checkEquals(combined1,combined2)
}

test.pass.anonymous.function <- function() {
  actual <- do.call(function(x,y)x +y,list(1:5, 5:1))
  expected <- rep(6,5)
  checkEquals(expected,actual)
}

test.closure <- function() {
  fool <- function(x) x+y
  # !!! comment below codes is not because that is wrong
  # !!! however, it is because it depends on some side-effect
  # !!! if the global environment happen to have one variable named "y"
  # !!! below line will fail
  # checkException(fool(1))
  
  y <- 100
  checkEqualsNumeric(101,fool(1))
  
  # dynamic binding, search every time
  # always reference the latest value
  y <- -1
  checkEqualsNumeric(0,fool(1))
}

test.local.shadow.parent <- function() {
  fool <- function(definelocal) {
    if (definelocal)
      # create a local variable
      # which will shadown the parent same-name variable
      y <- -1 
    y
  }
  
  y <- 100
  checkEqualsNumeric(100,fool(FALSE) )
  checkEqualsNumeric(-1,fool(TRUE) )
  
  y <- 1001
  checkEqualsNumeric(1001,fool(FALSE) )
  checkEqualsNumeric(-1,fool(TRUE) )
}

test.local.shadow.parent2 <- function() {
  fool <- function() {
    y <- y+1
    y*y
  }
  
  y <- 10
  
  # !!! this differs from Python
  # !!! in python, a local variable will be created in "fool"
  # !!! so an exception "reference an uninitialized variable"
  # !!! will be thrown
  checkEqualsNumeric( 121,fool() )
  checkEquals(10,y)
}





