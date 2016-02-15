


demo.dataframe.always.copy <- function(dt,n) {
  tracemem(dt)
  for (index in 1:n) {
    dt[1,1] <- rnorm(1)
  }
}

demo.matrix.change.in.place <- function(dt,n) {
  mat <- as.matrix(dt)
  tracemem(mat)
  
  for (index in 1:n) {
    mat[1,1] <- rnorm(1)
  }
}


dt <- data.frame(x=1:4,y=5:8)
print("initial data frame:")
dt

N <- 100

print("############### data.frame: always copy ###############")
demo.dataframe.always.copy(dt,N)
print("current data frame:")
dt


print("############### matrix: change in place ###############")
demo.matrix.change.in.place(dt,N)
print("current data frame:")
dt

