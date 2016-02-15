
library(RUnit)
library(plyr)

test.sort1 <- function() {
  x <-c(2, 32, 4, 16, 8)
  checkIdentical(c(2,4,8,16,32),sort(x))
  checkIdentical(c(32,16,8,4,2),sort(x,decreasing=TRUE))
  # the original vector is not changed
  checkIdentical(c(2, 32, 4, 16, 8),x)
}

# order is like "argsort" in NumPy
test.order.argsort <- function() {
  x <-c(2, 32, 4, 16, 8)
  sorted_index <- order(x)
  
  checkIdentical( c(1L,3L,5L,4L,2L),sorted_index )
  checkIdentical( sort(x),x[sorted_index] )
  
  inverse_sort_index <- order(x,decreasing=TRUE)
  checkIdentical( c(2L,4L,5L,3L,1L),inverse_sort_index )
  checkIdentical( sort(x,decreasing=TRUE),x[inverse_sort_index] )
}

test.sort.na <- function() {
  # the default value of parameter "na.last"==NA
  # which will remove NA
  x <-c(2, 32, NA,4, 16, 8)
  checkIdentical(c(2,4,8,16,32),sort(x))
  
  # na.last=TRUE, NA is put last position
  checkIdentical(c(2,4,8,16,32,NA),sort(x,na.last=TRUE))
  # na.last=FALSE, NA is put first position
  checkIdentical(c(NA,2,4,8,16,32),sort(x,na.last=FALSE))
}

test.plyr.arrange <- function() {
  dframe <- data.frame(
    x = c(1,4,5,2,3,0),
    y = c(9,4,8,1,6,7)
    )
  
  sorted_by_x <- dframe[order(dframe$x),]
  row.names(sorted_by_x ) <- 1:6
  checkIdentical(sorted_by_x,arrange(dframe,x))
  
  sorted_by_y <- dframe[order(dframe$y),]
  row.names(sorted_by_y ) <- 1:6
  checkIdentical(sorted_by_y,arrange(dframe,y))
}

# todo: reorder
