
library(RUnit)

create.samples <- function() {
  data.frame(
    id = 101:104,
    age = c(25, 34, 28, 52),
    gender =  factor(c("male","male","female","female")),
    diabete = factor(c("Type1", "Type2", "Type1", "Type1")),
    status = factor(c("Excellent","Poor", "Improved", "Poor"),
                    order=TRUE,
                    levels=c("Poor","Improved","Excellent")),
    
    # pay attention that, "." is necessary
    # row.names, cannot be "rownames", otherwise, you just create a new column called "rownames"
    row.names = c("tom","dick","mary","alice")
    )
}

test.create1 <- function() {
  id <- c(1,2,3,4)
  age <- c(25, 34, 28, 52)
  diabete <- factor(c("Type1", "Type2", "Type1", "Type1"))
  status <- factor(c("Poor", "Improved", "Excellent", "Poor"),
                   order=TRUE,
                   levels=c("Poor","Improved","Excellent"))
  patients <- data.frame(id,age,diabete,status)
  
  # the variable name of each column become the column name
  checkIdentical(c("id","age","diabete","status"),names(patients))
}

test.create2 <- function() {
  patients <- create.samples()
  
  # prefer using colnames, names is depreciated
  checkIdentical(c("id","age","gender","diabete","status"),names(patients))
  
  checkIdentical(c("id","age","gender","diabete","status"),colnames(patients))
  checkIdentical(c("tom","dick","mary","alice"),rownames(patients))
  
  checkIdentical(4L,nrow(patients))
  checkIdentical(5L,ncol(patients))
  checkIdentical(c(4L,5L),dim(patients))
}

test.is.list <- function(){
  # demonstrate that data.frame derives from list
  # and data.frame is a special list where each "field" 
  # has the same length
  d <- data.frame(x=1:5, y=6:10)
  checkIdentical("data.frame",class(d))
  checkTrue( is.list(d) )
}

test.index.slice <- function() {
  patients <- create.samples()
  
  expected <- data.frame(
    id = c(102L,103L),
    age = c(34,28),
    row.names = c("dick","mary")
    )
  
  checkIdentical(expected, patients[c(2,3),c("id","age")])
  checkIdentical(expected, patients[c("dick","mary"),c(1,2)])
  checkIdentical(expected, patients[c("dick","mary"),c("id","age")])
  checkIdentical(expected, patients[c(-1,-4),c("id","age")])
  checkIdentical(expected, patients[c(FALSE,TRUE,TRUE,FALSE),c("id","age")])
  checkIdentical(expected, patients[c(FALSE,TRUE,TRUE,FALSE),c(TRUE,TRUE,FALSE,FALSE,FALSE)])
}

# a single argument
# represents column not rows
test.index.single.arg <- function() {
  patients <- create.samples()
  
  expected <- data.frame(
    age = c(25, 34, 28, 52),
    gender =  factor(c("male","male","female","female")),
    row.names = c("tom","dick","mary","alice"))
  
  checkIdentical(expected, patients[2:3])
  checkIdentical(expected, patients[c("age","gender")])
}

test.access.dollar.sign <- function() {
  patients <- create.samples()
  checkIdentical( c(25, 34, 28, 52),patients$age )
  
  checkIdentical( c(103L,104L),patients$id[ patients$gender == "female" ] )
}

# subset is just a sugar
# it doesn't provide any special functions
# all its functions can be done by just passing in a logical vector
# the real advantage of using subset is saving you some typing
test.subset <- function() {
  patients <- create.samples()
  expected <- data.frame(
    id = 103:104,
    age = c(28,52),
    row.names = c("mary","alice"))
  actual <- subset(patients,gender=="female",c(id,age))
  checkIdentical(expected,actual)
}

test.rbind <- function() {
  patients <- create.samples()
  
  otherpatients <- data.frame(
    id = 51:52,
    age = c(45, 21),
    gender =  factor(c("male","female")),
    diabete = factor(c("Type1", "Type2")),
    status = factor(c("Improved", "Excellent"),
                    order=TRUE,
                    levels=c("Poor","Improved","Excellent"))
    )
  
  all <- rbind(otherpatients,patients)
  checkIdentical( c(51:52,101:104),all$id )
}

test.cbind <- function() {
  patients <- create.samples()
  
  # add one more column
  ssn = c(908L,89L,668L,698L)
  
  patients <- cbind(patients,ssn)
  checkIdentical(6L,ncol(patients))
  checkIdentical(ssn,patients$ssn)
  
  # append multiple columns
  otherattributes = data.frame(
    street = paste("street",1:4),
    city = paste("city",5:8),
    stringsAsFactors = FALSE
    )
  patients <- cbind(patients,otherattributes)
  checkIdentical("city 8",patients["alice","city"])
}

test.stringsAsFactors <- function() {
  # by default, string fields will automatically converted into factors
  factor_fields <- data.frame( x = c("a","b","c"))
  checkIdentical("factor",class(factor_fields$x))
  
  # disable the default options
  # treat string just as string
  string_fields <- data.frame( x = c("a","b","c"),stringsAsFactors=FALSE)
  checkIdentical("character",class(string_fields$x))
}

test.merge <- function() {
  d1 <- data.frame(
    id = 1:4,
    name = c("a","b","c","d"),
    stringsAsFactors = FALSE 
    )
  
  d2 <- data.frame(
    id = 3:5,
    score = c(66,88,99))
  
  merged <- merge(d1,d2,by="id")
  expected <- data.frame(
    id = 3:4,
    name = c("c","d"),
    score=c(66,88),
    stringsAsFactors = FALSE )
  checkIdentical(expected,merged)
  
  # not only those with overlapped id will be merged
  # but also those not overlapped will be added
  # however with NA to fill in the missing values
  merged.extra <- merge(d1,d2,by="id",all=TRUE)
  expected.extra <- data.frame(
    id = 1:5,
    name = c("a","b","c","d",NA),
    score = c(NA,NA,66,88,99),
    stringsAsFactors = FALSE 
    )
  checkIdentical(expected.extra,merged.extra)
}

test.col.math <- function() {
  d <- data.frame(
    score1 = 1:5,
    score2 = 6:10)
  
  filtered <- subset(d,score1%%2==0) 
  checkIdentical( c(score1=3,score2=8),colMeans(filtered) )
  checkIdentical( c(score1=6,score2=16),colSums(filtered) )
}

test.with <- function() {
  dt <- data.frame(
    x = 1:5,
    y = 6:10)
  actual <- with(dt,{
    sum <- x+y
    diff <- x-y
    sum * diff
  })
  expected <- c(-35L,-45L,-55L,-65L,-75L)
  checkIdentical(expected,actual)
}

test.within <- function() {
  dt <- data.frame(
    x = 1:5,
    y = 6:10)
  
  newdt <- within(dt,{
    sum <- x+y
    product <- x*y
  })
  
  # return a new copy
  checkIdentical(c(7L,9L,11L,13L,15L),newdt$sum)
  checkIdentical(c(6L,14L,24L,36L,50L),newdt$product)
  
  # the original dataframe is not changed
  checkIdentical(c(5L,2L),dim(dt))
  checkIdentical(1:5, dt$x)
  checkIdentical(6:10, dt$y)
}

test.transform <- function() {
  dt <- data.frame(x=1:4,y=5:8)
  
  # update
  newdt <- transform(dt,x=-x)
  checkIdentical(data.frame(x=-1:-4,y=5:8),newdt)
  
  # add new column
  newdt <- transform(newdt,z=x+y)
  checkIdentical(data.frame(x=-1:-4,y=5:8,z=rep(4L,4)),newdt)
}

