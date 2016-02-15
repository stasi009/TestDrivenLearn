
library(assertive)

demo.table1 <- function() {
  ages <- 16 + 50 *rbeta(10000, 2, 3)
  grouped_ages <- cut(ages,seq.int(16, 66, 10))
  
  assert_is_factor(grouped_ages)
  head(grouped_ages)
  
  tb <- table(grouped_ages)
  assert_is_table(tb)
  tb
}

demo.table2 <- function() {
  patients <- data.frame(
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
  with (patients,{
    print(table(gender,diabete))
    print(table(diabete,status))
  })
}
