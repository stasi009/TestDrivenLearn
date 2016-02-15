
library(lattice)

demo.lattice.scatter<- function() {
  xyplot(mpg~wt,mtcars,col = "red",pch = 20)
}

demo.lattice.update.scatter <- function() {
  (lat1 <- xyplot(mpg~wt,mtcars))
  (lat2 <- update(lat1,col = "violet",pch = 20))
}

demo1 <- function() {
  gear <- factor(mtcars$gear, levels = c(3, 4, 5), 
                 labels = c("3 gears", "4 gears", "5 gears"))
  
  cyl <- factor(mtcars$cyl, levels = c(4, 6, 8), 
                labels = c("4 cylinders", "6 cylinders", "8 cylinders"))

  densityplot(~mpg,data=mtcars,main = "Density Plot", xlab = "Miles per Gallon")
  
  densityplot(~mpg | cyl, data=mtcars,
      main = "Density Plot by Number of Cylinders", 
      xlab = "Miles per Gallon")
  
  bwplot(cyl ~ mpg | gear, data=mtcars,
      main = "Box Plots by Cylinders and Gears", 
      xlab = "Miles per Gallon", ylab = "Cylinders")
  
  xyplot(mpg ~ wt | cyl * gear, data=mtcars,
      main = "Scatter Plots by Cylinders and Gears", 
      xlab = "Car Weight", ylab = "Miles per Gallon")
  
  cloud(mpg ~ wt * qsec | cyl, data=mtcars,
      main = "3D Scatter Plots by Cylinders")
  
  dotplot(cyl ~ mpg | gear,data=mtcars, 
      main = "Dot Plots by Number of Gears and Cylinders", 
      xlab = "Miles Per Gallon")
  
  splom(mtcars[c(1, 3, 4, 5, 6)], 
      main = "Scatter Plot Matrix for mtcars Data")
}

demo.layout1 <- function() {
  # a vector of 4 integers, c(x,y,nx,ny) , 
  # that says to position the current plot at the x,y position in a regular array of nx by ny plots. 
  # (Note: this has origin at top left)
  graph1 <- histogram(~height|voice.part, data=singer,
                      main="Heights of Choral Singers by Voice Part")
  graph2 <- densityplot(~height, data=singer, group=voice.part,
                        plot.points=FALSE, auto.key=list(columns=4))
  plot(graph1, split=c(1, 1, 1, 2))
  plot(graph2, split=c(1, 2, 1, 2), newpage=FALSE)
}

demo.layout2 <- function() {
  graph1 <- histogram(~height|voice.part, data=singer,
                      main="Heights of Choral Singers by Voice Part")
  graph2 <- densityplot(~height, data=singer, group=voice.part,
                        plot.points=FALSE, auto.key=list(columns=4))
  plot(graph1, position=c(0, .3, 1, 1))
  plot(graph2, position=c(0, 0, 1, .3), newpage=FALSE)
}









