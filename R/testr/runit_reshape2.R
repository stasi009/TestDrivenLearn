
library(RUnit)
library(reshape2)


# wide dataframe --> long dataframe
demo.melt <- function() {
  wide_df <- data.frame(
    length = c("long","long","short","short"),
    time = c("soon","later","later","soon"),
    dead=c(84,15,48,99),
    alive = c(78,45,66,98)
    )
  
  # all columns other than "id.vars"
  # will be viewed as "measurements", and melt
  (long_df <- melt(wide_df,id.vars=c("length","time"),variable.name="survival",value.name="count"))
}

# long dataframe --> wide dataframe
demo.dcast <- function() {
  long_df <- data.frame(
    length = c("long","long","short","short","long","long","short","short"),
    time = c("atonce","later","atonce","later","atonce","later","atonce","later"),
    survival = c("dead","dead","dead","dead","alive","alive","alive","alive"),
    count = c(53,14,19,41,26,6,8,86)
    )
  ( wide_df <- dcast(long_df,length+time ~ survival,value.var="count") )
}