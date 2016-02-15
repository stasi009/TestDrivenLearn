
library(RUnit)

####################################################################
# class definition
point_generator <- setRefClass(
  "point",
  fields = list(
    x = "numeric",
    y = "numeric"),
  methods = list (
    initialize = function(x=NA_real_,y=NA_real_) {
      "constructor" # first string is regarded as help message
      # here, it is interesting that, even we use same name for paramter that passed in
      # and the member fields, R can know how to distinguish them
      x <<- x
      y <<- y
    },
    
    distanceFromOriginal = function() {
      "Euclidean distance from the origin"
      sqrt(x^2 + y^2)
    },
    
    add = function(another) {
      "Add another point to this point"
      x <<- x + another$x
      y <<- y + another$y
      .self
    }
    
    )# methods
  )

three_dim_point_generator <- setRefClass(
  "three_d_point",
  contains = "point",
  fields = list(z="numeric"),
  methods = list(
    initialize = function(x=NA_real_,y=NA_real_,z=NA_real_) {
      x <<- x
      y <<- y
      z <<- z
    },
    distanceFromOrigin = function()     {
      "Euclidean distance from the origin"
      two_d_distance <-callSuper()
      sqrt(two_d_distance ^ 2 +z ^ 2)
    }
    )
  )# three dimensional point

####################################################################
# test methods
test.sample1 <- function() {
  p1 <- point_generator$new(3,4)
  checkIdentical(3,p1$x)
  checkIdentical(4,p1$y)
  
  checkIdentical(5, p1$distanceFromOriginal())
  
  p2 <- point_generator$new(1,1)
  p1$add(p2)
  checkIdentical(4,p1$x)
  checkIdentical(5,p1$y)
}

test.getter.setter <- function() {
  p1 <- point_generator$new(3,4)
  checkIdentical(3,p1$x)
  checkIdentical(4,p1$y)
  
  p1$x <- 100
  checkIdentical(100,p1$x)
}

test.class <- function() {
  p2 <- point_generator$new(3,4)
  p3 <- three_dim_point_generator$new(1,1,1)
  
  checkIdentical("point", class(p2)[1])
  checkIdentical("three_d_point", class(p3)[1])
  
  checkTrue( inherits(p2,"point") )
  checkTrue( inherits(p3,"three_d_point") )
  checkTrue( inherits(p3,"point") )
}





