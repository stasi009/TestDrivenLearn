
demo.dir <- function() {
  # list all files with extension ".R" under current working directory
  r_files <-dir(pattern = "\\.R$")
  for (file in r_files) {
    print(file)
  }
}

