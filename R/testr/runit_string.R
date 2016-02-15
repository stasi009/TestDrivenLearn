
library(RUnit)
library(stringr)

test.str.detect <- function() {
  txts <- c("hello stasi from R","I love tingting")
  # fixed: means we are not using regular expression
  checkIdentical( c(FALSE,TRUE), str_detect(txts,fixed("ting")) )
}

test.nchar <- function() {
  txts <- c("cheka","kgb","gru","stasi")
  checkIdentical( c(5L,3L,3L,5L),nchar(txts) )
}

test.str.numeric <- function() {
  checkEqualsNumeric( 3.14,as.numeric("3.14") )
  checkIdentical("3.1415",as.character(3.1415))
}

test.paste <- function() {
  checkIdentical(c("a 1","b 2","c 3"), paste(c("a","b","c"),1:3))
  checkIdentical(c("a1","b2","c3"), paste(c("a","b","c"),1:3,sep=""))
  
  # collapse a string vector into one string
  checkIdentical("a1, b2, c3", paste(c("a","b","c"),1:3,sep="",collapse=", "))
}

test.concatenate <- function() {
  v <- 1:4
  checkIdentical( "1,2,3,4",paste(v,collapse=",") )
  checkIdentical( "1-2-3-4",paste(v,collapse="-") )
  checkIdentical( "1, 2, 3, 4",toString(v) )
}

test.tostring <- function() {
  # toStringis a variation of  pastethat is useful for printing vectors. It
  # separates each element with a comma and a space, and can limit how much we print
  x <- (1:15) ^ 2
  
  actual <-toString(x)
  expected <- "1, 4, 9, 16, 25, 36, 49, 64, 81, 100, 121, 144, 169, 196, 225"
  checkIdentical(expected,actual)
  
  actual <- toString(x,width = 40)
  expected <- "1, 4, 9, 16, 25, 36, 49, 64, 81, 100...."
  checkIdentical(expected,actual)
}

test.formatC <- function() {
  powers_of_e <- exp(1:3)
  
  checkIdentical(c("2.72","7.39","20.1"), 
                 formatC(powers_of_e,digits = 3) )
  
  checkIdentical( c("      2.72","      7.39","      20.1"),
                  formatC(powers_of_e,digits = 3,width = 10) )
  
  checkIdentical( c("2.718e+00","7.389e+00","2.009e+01"),
                  formatC(powers_of_e,digits = 3,format = "e") )
}

test.sprintf <- function() {
  n1 <- 1:4
  n2 <- 3:6
  texts <- sprintf("%d + %d = %d",n1,n2,n1+n2)
  checkIdentical(c("1 + 3 = 4","2 + 4 = 6","3 + 5 = 8","4 + 6 = 10"),texts)
}

test.prettyNum <- function() {
  texts <- prettyNum(
    c(1e10, 1e-20),
    big.mark  = ",",
    small.mark = " ",
    preserve.width = "individual",
    scientific = FALSE
  )
  checkIdentical(c("10,000,000,000","0.00000 00000 00000 00001"),texts)
}

test.upper.lower <- function() {
  s <- c("Stasi","Kgb")
  checkIdentical( c("stasi","kgb"),tolower(s) )
  checkIdentical( c("STASI","KGB"),toupper(s) )
}

test.substr.extract <- function() {
  s <- "Hello R in Sifang from Stasi"
  pos.start = 5
  pos.stop = 10
  checkIdentical( "o R in",substring(s,pos.start,pos.stop) )
  checkIdentical( "o R in",substr(s,pos.start,pos.stop) )
}

test.split <- function() {
  s <- "Hello R in Sifang from Stasi"
  # fixed=TRUE, indicates that we are using common string as separator
  # not regular expression
  seglist <- strsplit(s," ",fixed=TRUE)
  # even split a single string, it still returns a list
  checkTrue(is.list(seglist))
  checkIdentical( c("Hello","R","in","Sifang","from","Stasi"),seglist[[1]] )
  
  seglist <- strsplit(s,"R",fixed=TRUE)
  checkIdentical(c("Hello "," in Sifang from Stasi"),seglist[[1]])
}









