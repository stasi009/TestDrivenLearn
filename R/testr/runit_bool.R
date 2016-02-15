
library(RUnit)

test.sample1 <- function() {
  x <- 1:10 >= 5
  y <- 1:10 %% 2 == 0
  
  checkIdentical(c(FALSE,FALSE,FALSE,FALSE,FALSE,TRUE,FALSE,TRUE,FALSE,TRUE),x & y)
  checkIdentical(c(FALSE,TRUE,FALSE,TRUE,TRUE,TRUE,TRUE,TRUE,TRUE,TRUE),x | y)
}

