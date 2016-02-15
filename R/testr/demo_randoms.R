
library(assertive)

demo.samples1 <- function() {
  happy_choices <-c("depressed", "grumpy", "so-so", "cheery", "ecstatic")
  happy_values <-sample(
    happy_choices,
    10000,
    replace = TRUE
  )
  happy_fac <-factor(happy_values,ordered=TRUE,levels=happy_choices)
  head(happy_fac)
}

demo.sample2 <- function() {
  # you just pass it a   number, n, 
  # it will return a permutation of the natural numbers from 1 to n
  sample(10)
  
  # If you give it a second value, 
  # it will return that many random numbers between 1 and  n
  sample(10,3)
  
  # By default, samplesamples without   replacement. That is, each value can only appear once. 
  # To allow sampling with replacement, pass replace = TRUE
  sample(10,10,replace=TRUE)
  
  # sample within a vector
  sample(colors(), 5)
  
  # sample with weights
  weights <-c(1, 1, 2, 3, 5, 8, 13, 21, 8, 3, 1, 1)
  sample(month.abb, 1,prob =weights)
}

demo.set.seed <- function() {
  set.seed(1)
  replicate(5,runif(5))
  
  set.seed(1)
  replicate(5,runif(5))
}