
library(RUnit)

test.as.logical <- function() {
  # only 0 is regarded as FALSE
  # all the other values, no matter positive or negative, can be viewed as TRUE
  number_vec <- c(0,1,2,-1)
  checkIdentical(c(FALSE,TRUE,TRUE,TRUE),sapply(number_vec,as.logical))
  
}

test.add.or <- function() {
  x <- c(TRUE,FALSE)
  y <- c(FALSE,FALSE)
  z <- c(TRUE,TRUE)
  
  # & and | are for element-wise, return another vector
  checkIdentical( c(FALSE,FALSE),x & y )
  checkIdentical( c(TRUE,FALSE),x & z )
  checkIdentical( c(TRUE,FALSE),x | y )
  checkIdentical( c(TRUE,TRUE),x | z )
  
  # && and || return a single logical value
  # when passed in a vector, only the first element is checked
  checkIdentical( FALSE,x && y )
  checkIdentical( TRUE,x && z )
  checkIdentical( TRUE,x || y )
  checkIdentical( TRUE,x || z )
}

test.all.any <- function () {
  allconditions <- list(
    all_false = c(FALSE, FALSE, FALSE),
    some_true = c(FALSE, TRUE, FALSE),
    all_true = c(TRUE, TRUE, TRUE)
    )
  
  expected_allresult <- list(
    all_false = FALSE,
    some_true = FALSE,
    all_true = TRUE
    )
  checkIdentical(expected_allresult,lapply(allconditions,all))
  
  expected_anyresult <- list(
    all_false = FALSE,
    some_true = TRUE,
    all_true = TRUE
  )
  checkIdentical(expected_anyresult,lapply(allconditions,any))
}