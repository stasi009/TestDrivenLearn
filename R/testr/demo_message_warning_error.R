
demo.message <- function() {
  fool <- function(x)
  {
    message("'x' contains ",toString(x))
    x
  }
  fool(1:5)
  suppressMessages(fool(c(8:10)))
}

demo.warning <- function() {
  fool <- function(x)
  {
    warning("'x' contains ",toString(x))
    x
  }
  fool(1:5)
  suppressWarnings(fool(c(8:10)))
}

demo.finally <- function() {
  fool <- function(x) {
    if (x%%2 == 0) {
      x
    } else {
      stop(sprintf("%d is not even",x))
    }
  }# fool
  
  safe.fool <- function(x) {
    tryCatch(fool(x),
             error = function(e) {
               warning(e$message)
               -1
             },
             finally = message("finally is invoked")
    )
  }
  
  safe.fool(2)
  safe.fool(3)
}