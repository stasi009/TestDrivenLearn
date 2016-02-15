
library(RUnit)

test.index <- function() {
  l <- list(
    first = 1,
    second = 2,
    third = list(
      alpha = 9.9,
      beta = 8.8
      ))
  
  # difference between [] and [[]]
  checkIdentical( "list",class( l[1]) )
  checkIdentical(list(first=1),l[1])
  checkTrue(is.list(l[1]))
  checkIdentical(list(first=1),l["first"])
  
  checkIdentical( "numeric",class( l[[1]]) )
  checkIdentical( 1,l[[1]] )
  checkIdentical( 1,l[["first"]] )
  
  # access by name
  checkIdentical(1,l$first)
  checkIdentical(2,l$second)
}

test.nested.index <- function() {
  l <- list(
    first = 1,
    second = 2,
    third = list(
      alpha = 9.9,
      beta = 8.8
    ))
  
  checkEqualsNumeric(9.9, l$third$alpha)
  checkEqualsNumeric(9.9,l[[c("third","alpha")]])
  checkEqualsNumeric(8.8, l[[3]][["beta"]])
  checkEqualsNumeric(8.8, l[["third"]][["beta"]])
}

test.nonexist.index <- function() {
  # the behaviour when accessing non-existent index 
  # is totally non-consistent
  l <- list(a=1,b=2)
  checkException( l[[4]],"subscript out of bounds")
  checkTrue( is.null(l[["x"]]) )
}

test.vector.list <- function() {
  v <- c(1,2,3)
  l <- as.list(v)
  checkIdentical( 2,l[[2]] )
  
  # if the list contain only scalar element
  # we can use as.xxx to convert it to vector
  # the order of the vector only depends on the declaration order
  checkIdentical( c(1,2),as.numeric(list(a=1,b=2)) )
  checkIdentical( c(2,1),as.numeric(list(b=2,a=1)) )
}

test.unlist <- function() {
  l <- list(
    first = 1,
    second = 2,
    third = list(
      alpha = 9.9,
      beta = 8.8
    ))
  
  expected <- c(first=1,
                second = 2,
                third.alpha=9.9,
                third.beta = 8.8)
  
  checkIdentical(expected,unlist(l))
}

test.combine.list <- function() {
  checkIdentical(list(a=1,b=2,3,4),
                 c(list(a=1,b=2),list(3,4)),
                 "concatenate list")
  
  checkIdentical(list(a=1,b=2,3),
                 c(list(a=1,b=2),3),
                 "concatenate list")
}

test.remove.by.setnull <- function() {
  l <- list(a=1,b=2,c=3)
  l$b <- NULL
  checkIdentical(list(a=1,c=3),l)
}






