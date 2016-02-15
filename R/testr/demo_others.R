
demo.correct.if.else <- function() {
  if (runif(1) > 0.5){
    message("+++ coin is head")
  } else{
    message("--- coin is tail")
  }
}

demo.wrong.if.else <- function() {
  if (runif(1) > 0.5){
    message("+++ coin is head")
  } # "else" must be in the same line as }, otherwise, it is an error
  else{
    message("--- coin is tail")
  }
}

demo.view() <- function() {
  library(datasets)
  View(airquality)
}

demo.edit() <- function() {
  mydata <- data.frame(age=numeric(0),                        
                       gender=character(0), 
                       weight=numeric(0))
  mydata <- edit(mydata) 
  # fix(mydata) is just equivalent to "mydata <- edit(mydata) "
  # fix is just more convenient
  fix(mydata)
}