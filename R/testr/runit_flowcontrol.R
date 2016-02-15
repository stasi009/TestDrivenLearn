
library(RUnit)

test.if.else <- function() {
  fool <- function(x) {
    if(is.na(x))
    {
      "missing"
    } else if(is.infinite(x))
    {
      "infinite"
    } else if(x > 0)
    {
      "positive"
    } else if(x < 0)
    {
      "x is negative"
    } else
    {
      "x is zero"
    }
  }
  
  checkIdentical("missing", fool(NA) )
  checkIdentical("positive", fool(9) )
}

test.if.else.assignment <- function() {
  x <- 10
  y <- if (x%%2==0) "even" else "odd"
  checkIdentical("even",y)
}

test.ifelse <- function() {
  checkIdentical(c("odd","even","odd","even","odd"), 
                 ifelse((1:5)%%2==0,"even","odd"))
  
  x <- c(1,2,3,4,5)
  y <- c(6,7,8,9,10)
  checkIdentical(c(1,7,8,4,10), ifelse(c(TRUE,FALSE,FALSE,TRUE,FALSE),x,y))
  
  # if the logical vector has NA
  # then corresponding elements in the result vector will also be NA
  checkIdentical(c(1,7,NA,4,10), ifelse(c(TRUE,FALSE,NA,TRUE,FALSE),x,y))
}

test.switch <- function () {
  fool <- function(x) {
    switch (
      x,
      alpha = 1,
      beta = sqrt(4),
      gamma = {
        a <-sin(pi / 3)
        4 *a ^ 2
      },
      -1 # default value
      )# switch
  }# fool
  
  actual <- sapply(c("beta","gamma","not existed"),fool)
  expected <- c(beta=2,gamma=3,"not existed"=-1)
  checkEquals(expected,actual)
}



