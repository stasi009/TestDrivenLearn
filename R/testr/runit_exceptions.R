
library(RUnit)
library(assertive)
library(plyr)

test.stop <- function() {
  fool <- function(x,na.rm = FALSE)
  {
    if(!na.rm && any(is.na(x)))
    {
      stop("'x' has missing values.")
    }
    x
  }
  
  fool(1:4)
  checkException( fool(c(1,NA)) )
}

test.stopifnot <- function() {
  fool <- function(x,na.rm = FALSE)
  {
    if(!na.rm)
    {
      stopifnot(!any(is.na(x)))
    }
    x
  }
  fool(1:4)
  checkException( fool(c(1,NA)) )
}

test.assert <- function() {
  fool <- function(x,na.rm = FALSE)
  {
    if(!na.rm)
    {
      assert_all_are_not_na(x)
    }
    x
  }
  checkException( fool(c(1, NA)) )
}

test.trycatch <- function() {
  fool <- function(x) {
    if (x%%2 == 0) {
      x
    } else {
      stop(sprintf("%d is not even",x))
    }
  }# fool
  checkIdentical(2, fool(2) )
  
  compromised <- tryCatch(fool(3),error = function(e) {
    warning(e$message)
    -1
  })
  checkIdentical(-1,compromised)
}

test.tryapply <- function() {
  fool <- function(x) {
    if (x%%2==0) {
      -x
    } else {
      stop(sprintf("%d is odd",x))
    }
  }
  # those failed to convert will not appear in the result of tryapply
  checkIdentical( c(-2L,-4L),unlist(tryapply(1:4,fool)) )
}
