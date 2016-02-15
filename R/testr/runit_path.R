
library(RUnit)

test.combine.path <- function() {
  actual <- file.path("c:", "Program Files", "R", "R-devel")
  expected <- "c:/Program Files/R/R-devel"
  checkIdentical(expected,actual)
}

test.dir.base.name <- function() {
  full_file_name <-  "C:/Program Files/R/R-devel/bin/x64/RGui.exe"
  checkIdentical("RGui.exe",basename(full_file_name))
  checkIdentical("C:/Program Files/R/R-devel/bin/x64",dirname(full_file_name))
}

