

################################ scatter ################################
demo.base.scatter <- function() {
  pntshape <- 25
  with(mtcars,plot(mpg,wt,col = "violet",pch = pntshape))
}

################################ line ################################
demo.line1 <- function() {
  # "b" means both point and line
  with(mtcars,plot(mpg,wt,type="b", lty=2, pch=17))
  
  # configure the plot
  opar <- par(no.readonly=TRUE)
  par(lty=2, pch=11) 
  with(mtcars,plot(mpg,wt,type="b", lty=2, pch=11))
  par(opar) 
}

demo.line2 <- function() {
  dose <- c(20, 30, 40, 45, 60)
  drugA <- c(16, 20, 27, 40, 60)
  drugB <- c(15, 18, 25, 31, 40)
  
  opar <- par(no.readonly=TRUE) 
  
  par(lwd=2, cex=1.5, font.lab=2) 
  plot(dose, drugA, type="b", 
       pch=15, lty=1, col="red", ylim=c(0, 60), 
       main="Drug A vs. Drug B", 
       xlab="Drug Dosage", ylab="Drug Response") 
  lines(dose, drugB, type="b", 
        pch=17, lty=2, col="blue") 
  abline(h=c(30), lwd=1.5, lty=2, col="gray") 
  par(opar)
  
  # legend("topleft", title="Drug Type", c("A","B"), lty=c(1, 2), col=c("red", "blue")) 
  legend(locator(1), title="Drug Type", c("A","B"), lty=c(1, 2), col=c("red", "blue")) 
}

################################ multiple plots ################################
demo.multiple.plots <- function() {
  with(mtcars,{
    opar <- par(no.readonly=TRUE)
    par(mfrow=c(2,2))
    plot(wt,mpg, main="Scatterplot of wt vs. mpg")
    plot(wt,disp, main="Scatterplot of wt vs disp")
    hist(wt, main="Histogram of wt")
    boxplot(wt, main="Boxplot of wt")
    par(opar)
    
  }    )
}

demo.layout <- function() {
  with (mtcars, {
    layout(matrix(c(1,2,3,2), 2, 2, byrow = TRUE))
    hist(wt)
    hist(mpg)
    hist(disp)
  })
}

demo.layout2 <- function() {
  with (mtcars,{
    layout(matrix(c(1, 1, 2, 3), 2, 2, byrow = TRUE), 
           widths=c(3, 1), heights=c(1, 2))
    hist(wt)
    hist(mpg)
    hist(disp)
  })
}

################################ misc ################################
demo.identify <- function() {
  plot(mtcars$wt, mtcars$mpg)
  grid()
  identify(mtcars$wt, mtcars$mpg, labels=row.names(mtcars))
}






