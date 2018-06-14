# Varians versus repeats
reachData <- read.csv("~/Dropbox/Thesis/Data/Logs/variansData.csv", header = TRUE)

nd <- reachData[c("ProtocolType","FoundPercentage")]
nd <- subset(nd, nd$ProtocolType != "BLA" & nd$ProtocolType != "ZRPP4")
nd$ProtocolType <- factor(nd$ProtocolType)
n <- aggregate(FoundPercentage ~ ProtocolType, data = nd, mean)

for (i in 1:length(nd[,1])) {
  tmp <- subset(n, n$ProtocolType==nd[i,]$ProtocolType)
  tmp2 <- nd[i,]$FoundPercentage
  nd[i,]$FoundPercentage <- tmp2-tmp$FoundPercentage
}

plot(nd$ProtocolType, nd$FoundPercentage, las = 2, ylim=c(-2,2), xlab="", ylab="Percentage %")

# Over TTL
reachData <- read.csv("~/Dropbox/Thesis/Data/Logs/variansData.csv", header = TRUE)
reachData <- subset(reachData, reachData$ProtocolType!="BLA" & reachData$TTL != 4)
nd <- reachData[c("ProtocolType","FoundPercentage", "TTL")]
n <- aggregate(FoundPercentage ~ ProtocolType, data = nd, mean)

for (i in 1:length(nd[,1])) {
  tmp <- subset(n, n$ProtocolType==nd[i,]$ProtocolType)
  tmp2 <- nd[i,]$FoundPercentage
  nd[i,]$FoundPercentage <- tmp2-tmp$FoundPercentage
}
nd$TTL <- factor(nd$TTL)
plot(nd$TTL, nd$FoundPercentage, las = 1, ylim=c(-2,2), xlab="TTL", ylab="Variance in %")

# Over repeats
reachData <- read.csv("~/Dropbox/Thesis/Data/Logs/variansData.csv", header = TRUE)
reachData <- subset(reachData, reachData$ProtocolType!="BLA")

par(mfrow=c(1,2))
nd <- reachData[c("ProtocolType","FoundPercentage", "Repeats")]
n <- aggregate(FoundPercentage ~ ProtocolType, data = nd, mean)

for (i in 1:length(nd[,1])) {
  tmp <- subset(n, n$ProtocolType==nd[i,]$ProtocolType)
  tmp2 <- nd[i,]$FoundPercentage
  nd[i,]$FoundPercentage <- tmp2-tmp$FoundPercentage
}
nd$Repeats <- factor(nd$Repeats)
plot(nd$Repeats, nd$FoundPercentage, las = 1, ylim=c(-2,2), xlab="", ylab="Variance in %", main = "All protocols")

nd <- reachData[c("ProtocolType","FoundPercentage", "Repeats")]
nd <- subset(nd, nd$ProtocolType != "DSR0" & nd$ProtocolType != "DSR1" & nd$ProtocolType != "DSR2" & nd$ProtocolType != "DSR3")
n <- aggregate(FoundPercentage ~ ProtocolType, data = nd, mean)

for (i in 1:length(nd[,1])) {
  tmp <- subset(n, n$ProtocolType==nd[i,]$ProtocolType)
  tmp2 <- nd[i,]$FoundPercentage
  nd[i,]$FoundPercentage <- tmp2-tmp$FoundPercentage
}
nd$Repeats <- factor(nd$Repeats)
plot(nd$Repeats, nd$FoundPercentage, las = 1, ylim=c(-2,2), xlab="", ylab="", main = "Without DSR")
mtext("Repeats", side=1, padj=-4, outer=TRUE)


