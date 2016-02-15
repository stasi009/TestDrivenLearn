

demo.repeat <- function() {
  repeat
  {
    message("Happy Groundhog Day!")
    action <-sample(
      c(
        "Learn French",
        "Make an ice statue",
        "Rob a bank",
        "Win heart of Andie McDowell"
      ),
      1
    )
    if(action == "Rob a bank")
    {
      message("Quietly skipping to the next iteration")
      next
    }
    message("action = ",action)
    if(action == "Win heart of Andie McDowell") break
  }
}

demo.while <- function() {
  decide.action <- function() {
    sample(
      c(
        "Learn French",
        "Make an ice statue",
        "Rob a bank",
        "Win heart of Andie McDowell"
      ),      1     )
  }
  
  action <- decide.action()
  while(action != "Win heart of Andie McDowell")
  {
    message("Happy Groundhog Day!")
    action <- decide.action()
    message("action = ",action)
  }
}

demo.for <- function() {
  for(i in 1:5)
  {
    j <-i ^ 2
    message("j = ",j)
  }
  
  for(month in month.name)
  {
    message("The month of ",month)
  }
}

demo.rep.replicate <- function() {
  # the runif(1) will be evaluated first only once
  # so it will produce a vector with the same element
  rep(runif(1), 5)
  
  # runif(1) will be evaluated everytime
  # so a new random value will be generated each time
  replicate(5,runif(1))
}




