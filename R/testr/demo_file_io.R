
make.dataframe <- function() {
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

"
read.csv  sets the default separator to a comma, and assumes that the data has a header row. 
read.csv2 is its European cousin, using a comma for decimal places and a semicolon as a separator.
"
demo.read.csv1 <- function() {
  who <- read.csv("who.csv",header=TRUE,fill=TRUE)
  
  sample_data1 <- read.csv("sample_data.csv")
  print(sample_data1)
  
  # we designate "id" become the "case identifier"
  # it will not be in the columns any more
  sample_data2 <- read.csv("sample_data.csv",row.names = "id")
  print(sample_data2)
}

demo.read.table1 <- function() {
  filename <- "who.csv"
  who <- read.table(filename,sep=",",header=TRUE,fill=TRUE,stringsAsFactors=FALSE)
}

demo.write.csv <- function() {
  dt <- make.dataframe()
  # pay attention to "row.names=FALSE"
  # if you doesn't specify row.names, then row.names will be row numbers
  # you don't want to print row.numbers
  # so you have to specify "row.names=FALSE"
  write.csv(dt,"sample_data.csv",row.names=FALSE)
}

demo.save <- function() {
  dt1 <- make.dataframe()
  dt2 <- data.frame(
    x = 1:4,
    y = 5:8)
  save(dt1,dt2,file="sample_data.rdata")
}

demo.load <- function() {
  env <- new.env()
  load("sample_data.rdata",envir=env)
  ls(env=env)
  
  head(env$dt1)
  head(env$dt2)
}
