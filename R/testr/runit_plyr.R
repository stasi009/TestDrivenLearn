
library(RUnit)
library(plyr)

demo.ddply.transform <- function() {
  library(MASS) # For the data set
  # we want to normalize the data within each group by finding the deviation of each case from
  # the mean within the group
  # First it splits cabbages into separate data frames based on the value of Cult. 
  # It then applies the transform()function, with the remaining arguments, to each data frame.
  ddply(cabbages, "Cult",transform,DevWt =HeadWt -mean(HeadWt))  
  
  # split by Cultand Date, forming a group for each unique combination of the two variables, 
  # and then it will calculate the deviation from the mean of HeadWtand VitCwithin each group
  ddply(cabbages,c("Cult", "Date"),transform,
        DevWt =HeadWt -mean(HeadWt),DevVitC =VitC -mean(VitC))
}

demo.ddply.summarize <- function() {
  library(MASS) # For the data set
  # summarize for each unique combination of "cult" and "date"
  ddply(cabbages,c("Cult", "Date"),summarise,Weight =mean(HeadWt),
        VitC =mean(VitC))
  
  partcb <- cabbages[c(1:5,55:60),]
  # the difference between "transform" and "summarize"
  # is that: summarize will return a dataframe with only one row
  ddply(partcb, "Cult",summarize,Weight =mean(HeadWt))
  ddply(partcb, "Cult",transform,Weight =mean(HeadWt))
}

test.sample1 <- function() {
  d <- data.frame(x=1:5, y=6:10)
  
  # input: list
  # output: list
  checkIdentical( list(x=15L,y=40L), llply(d,sum) )
  
  # input: list
  # output: array
  checkIdentical( c(15L,40L), laply(d,sum) )
}

test.mutate <- function() {
  dt <- data.frame(
    x = 1:5,
    y = 6:10)
  
  newdt <- mutate(dt,
                  sum = x+y,
                  product = x*y)
  checkIdentical(c(7L,9L,11L,13L,15L),newdt$sum)
  checkIdentical(c(6L,14L,24L,36L,50L),newdt$product)
}

test.ddply.colwise <- function() {
  d <- data.frame(
    name = c("a","b","a","c", "c","b","a"),
    score = 1:7,
    level =  c(4,5,2,1,3,5,8))
  
  actual <- ddply(d,.(name),colwise(mean))
  expected <- data.frame(
    name = c("a","b","c"),
    score = c(3.666667,4,4.5),
    level = c(4.666667,5,2))
  checkEquals(expected,actual,tolerance=1e-4)
}

# ddply like base R's tapply
# but with advanced functions
test.ddply.summarize <- function() {
  d <- data.frame(
    name = c("a","b","a","c", "c","b","a"),
    score = 1:7,
    level =  c(4,5,2,1,3,5,8))
  
  actual <- ddply(d,.(name),
                  summarize,
                  mean_score = mean(score),
                  max_level = max(level))
  expected <- data.frame(
    name = c("a","b","c"),
    mean_score = c(3.666667,4,4.5),
    max_level = c(8,5,3))
  checkEquals(expected,actual,tolerance=1e-4)
}

test.ddply.userdeffunc <- function() {
  # accept a dataframe, and return a datafram
  mapper <- function(indata) {
    data.frame(mean_score = mean(indata$score),max_level=max(indata$level))
  }
  
  d <- data.frame(
    name = c("a","b","a","c", "c","b","a"),
    score = 1:7,
    level =  c(4,5,2,1,3,5,8))
  
  actual <- ddply(d,"name",mapper)
  
  expected <- data.frame(
    name = c("a","b","c"),
    mean_score = c(3.666667,4,4.5),
    max_level = c(8,5,3))
  checkEquals(expected,actual,tolerance=1e-4)
}














