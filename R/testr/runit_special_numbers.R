
library(RUnit)

test.math.on.special <- function() {
  checkEquals(0,1/Inf)
  checkEquals(0,1/-Inf)
  
  checkTrue(is.nan(0/0))
}

test.is.finite <- function() {
  x <- c(0,Inf, -Inf, NaN, NA)
  # NAN, NA neither finite, nor infinite
  checkIdentical( c(TRUE,FALSE,FALSE,FALSE,FALSE), is.finite(x) )
  checkIdentical( c(FALSE,TRUE,TRUE,FALSE,FALSE), is.infinite(x) )
}

test.isnan <- function() {
  x <- c(0,Inf, -Inf, NaN, NA)
  checkIdentical( c(FALSE,FALSE,FALSE,TRUE,FALSE), is.nan(x))
}

test.isna <- function() {
  x <- c(0,Inf, -Inf, NaN, NA)
  checkIdentical( c(FALSE,FALSE,FALSE,TRUE,TRUE), is.na(x))
  
  checkTrue(!is.nan(NA) )# NA is not NaN
  checkTrue(is.na(NaN) ) # NaN is NA
}

test.isnull <- function() {
  # NUll and NA have totally different meanings
  # NA means Not Available, representing missing values
  # NULL means "empty". Not missing, just contain nothing
  checkTrue(is.null(NULL))
  checkTrue(!is.null(NA))
}