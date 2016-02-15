
library(RUnit)

test.class <- function() {
  variables <- list(
    float_number = 3.14,
    int_number = 3L,
    int_as_float = 3,
    txt = "stasi",
    condition = TRUE,
    int_vector = 6:8,
    float_vector = c(6,7,8),
    float_vector2 = 6.1:9,# if one is not whole number, then it will be numeric
    complex_number = 1+2i
  )
  actual <- lapply(variables,class)
  
  expected <- list(
    float_number = "numeric",
    int_number = "integer",
    int_as_float = "numeric",
    txt = "character",
    condition = "logical",
    int_vector = "integer",
    float_vector = "numeric" ,
    float_vector2 = "numeric",
    complex_number = "complex"
  )
  checkIdentical(expected,actual)
}

test.is.generic <- function() {
  # pay attention that, we cannot use vector to store variables
  # because vector can only store same type of elements
  # so we have to use list here
  variables <- list(9L,9,TRUE,"stasi",3+4i)
  types <- c("integer","numeric","logical","character","complex")
  checkTrue(all(mapply(is,variables,types)))
}

test.is.specific <- function() {
  int_number = 9L
  checkTrue(is.integer(int_number))
  checkTrue(is.numeric(int_number))
  checkTrue(!is.double(int_number))
  
  float_number = 9
  checkTrue(!is.integer(float_number))
  checkTrue(is.numeric(float_number))
  checkTrue(is.double(float_number))
  
  checkTrue(is.character("stasi"))
  checkTrue(is.logical(FALSE))
}

test.as.numeric.string <- function() {
  checkEqualsNumeric( 3.14,as.numeric("3.14") )
  checkIdentical("3.1415",as.character(3.1415))
}


