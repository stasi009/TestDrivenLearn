
# the difference between array(matrix) and list(data.frame)
# is that: array can only contain one type of data
# while list can store different types of data

library(RUnit)

test.create.matrix <- function() {
  matrix1 <- matrix(1:12,nrow=4)
  checkIdentical("matrix",class(matrix1))
  checkIdentical( c(4L,3L),dim(matrix1) )
  
  # by default, numbers are filled into matrix by column
  checkIdentical( c(1L,5L,9L),matrix1[1,] )
  checkIdentical( c(9L,10L,11L,12L),matrix1[,3])
  
  # fill the matrix by row
  matrix2 <- matrix(1:12,nrow=4,byrow=TRUE)
  checkIdentical( c(1L,2L,3L),matrix2[1,] )
  checkIdentical( c(3L,6L,9L,12L),matrix2[,3])
}

test.vec2matrix.assign.dim <- function() {
  v <- 1:20
  dim(v) <- c(4,5)
  checkTrue( is.matrix(v) )
  
  m <- matrix(1:20,nrow=4)
  
  checkIdentical(m,v)
}

test.dims <- function() {
  matrix1 <- matrix(1:12,nrow=4)
  checkIdentical( c(4L,3L),dim(matrix1) )
  checkIdentical(4L,nrow(matrix1))
  checkIdentical(3L,ncol(matrix1))
  checkIdentical(12L,length(matrix1))
}

test.reshape.by.reassign.dims <- function() {
  matrix1 <- matrix(1:12,nrow=4)
  checkIdentical(c(4L,3L),dim(matrix1))
  
  dim(matrix1) <- c(6,2)
  checkIdentical(c(6L,2L),dim(matrix1))
  checkIdentical(c(1L,7L),matrix1[1,])
  
  # if the new dimension doesn't match the length
  # an error will be raised
  checkException( dim(matrix1) <- c(2,3) )
}

test.names.get <- function() {
  rownames <- paste("row",1:4,sep="")
  colnames <- paste("col",1:3,sep="")
  matrix1 <- matrix(1:12,nrow=4,dimnames = list(rownames,colnames))
  
  checkIdentical(rownames,rownames(matrix1))
  checkIdentical(colnames,colnames(matrix1))
  checkIdentical(list(rownames,colnames),dimnames(matrix1))
}

test.index.slice1 <- function() {
  rownames <- paste("row",1:4,sep="")
  colnames <- paste("col",1:3,sep="")
  wholematrix <- matrix(1:12,nrow=4,dimnames = list(rownames,colnames))
  
  # mix the way to index
  checkEquals( matrix(c(2,3,10,11),nrow=2),
               wholematrix[c(2,3),c("col1","col3")],
               check.attributes = FALSE,
               checkNames = FALSE)
}

test.index.slice2 <- function() {
  rownames <- paste("row",1:2,sep="")
  colnames <- paste("col",1:3,sep="")
  wholematrix <- matrix(c(9,8,7,6,1,2),nrow=2,dimnames = list(rownames,colnames))
  
  # a single index just treat the matrix as a vector
  checkTrue( wholematrix[3] == 7)
  checkEquals(matrix(c(9,8,1,2),nrow=2), 
              wholematrix[,c("col1","col3")],
              check.attributes=FALSE)
  
  checkEquals( c(8,6,2),wholematrix[c("row2"),],check.attributes=FALSE)
  checkEqualsNumeric( c(8,6,2),wholematrix[c("row2"),])
}

test.concatenate<- function() {
  # first convert matrix to vector (column wise), and then concatenate
  # and return vector too
  m1 <- matrix(1:4,nrow=2)
  m2 <- matrix(5:8,nrow=2)
  
  checkEquals( 1:8, c(m1,m2))
  checkEquals( matrix(1:8,nrow=2),cbind(m1,m2) )
  checkEquals( matrix(c(1,2,5,6,3,4,7,8),nrow=4),rbind(m1,m2) )
}

test.elementwise.math <- function() {
  m1 <- matrix(1:4,nrow=2)
  m2 <- matrix(5:8,nrow=2)
  
  checkEquals( matrix(c(6,8,10,12),nrow=2), m1+m2 )
  
  # element-wise multiply
  checkEquals( matrix(c(5,12,21,32),nrow=2), m1*m2 )
  
  # dimension not match, it will throw an exception
  dim(m2) <- c(4,1)
  checkException( m1+m2,"mismatch dimensions will thrown an exception")
}

test.matrix.multiply <- function() {
  m1 <- matrix(1:6,nrow=2)
  m2 <- matrix(7:12,nrow=3)
  checkException( m1 * m2,"non-conformable for element-wise multiply")
  
  checkEquals(matrix(c(76,100,103,136),nrow=2),m1 %*% m2)
  checkEquals(matrix(c(27,30,33,61,68,75,95,106,117),nrow=3),m2 %*% m1)
}

test.matrix.vector.multiply <- function() {
  # matrix can multiply vector directly
  # no need to convert vector to matrix first
  m <- matrix(1:4,nrow=2)
  v <- 5:6
  
  elementwise_product <- matrix(c(5,12,15,24),nrow=2)
  checkEquals(elementwise_product,m * v)
  
  algebra_product <- matrix(c(23,34),nrow=2)
  checkEquals(algebra_product,m %*% v)
}

test.vector.inner.product <- function() {
  v1 <- 1:3
  v2 <- 4:6
  
  inner_product <- v1 %*% v2
  checkIdentical("matrix",class(inner_product))
  checkIdentical(c(1L,1L),dim(inner_product))
  
  scalar <- drop(inner_product)
  checkTrue(32 == scalar)
}

test.vector.outer.product <- function() {
  v1 <- 1:3
  v2 <- 4:6
  
  outer_product <- v1 %o% v2
  expected <- matrix(c(4,8,12,5,10,15,6,12,18),nrow=3)
  checkEquals(expected,outer_product)
  
  checkEquals(t(expected),v2 %o% v1)
}

test.transpose <- function() {
  m <- matrix(1:4,nrow=2)
  checkIdentical( matrix(c(1L,3L,2L,4L),nrow=2),t(m))
}

test.inverse <- function() {
  m <- matrix(1:4,nrow=2)
  inverse_m <- solve(m)
  
  unit_matrix = diag(rep(1,2))
  checkEquals(unit_matrix,m %*% inverse_m)
  checkEquals(unit_matrix,inverse_m %*% m)
}





