
# Reachability in original network
reachData <- read.csv("~/Dropbox/Thesis/Data/Logs/reachDataOriginal-Save.csv", header = TRUE)

dsr <- subset(reachData, reachData$ProtocolType=="DSR" & reachData$Repeats==3)
zrp <- subset(reachData, reachData$ProtocolType=="ZRP" & reachData$Repeats==3)
zrpp <- subset(reachData, reachData$ProtocolType=="ZRP-PRUNE" & reachData$Repeats==3)

plot(NULL,NULL, xlim = c(1.1,4), ylim = c(2,100), xaxt="n", xlab = "TTL", ylab = "Reach %", main = "Reachability in original network")
abline(h=reachData$FoundPercentage[1], col="black", lwd = 3)
lines(dsr$TTL, dsr$FoundPercentage, col="blue", lwd = 3)
lines(zrp$TTL, zrp$FoundPercentage, col="red", lwd = 3)
lines(zrpp$TTL, zrpp$FoundPercentage, col="green", lwd = 3)
axis(side = 1, at=c(1:4))

legend("bottomright", legend=c("DSR", "ZRP", "ZRP-Prune", "DSDV"),
       col=c("blue", "red", "green", "black"), lty=1, cex=0.8, lwd=2)


# Reachbility in AddIn network
reachData <- read.csv("~/Dropbox/Thesis/Data/Logs/reachDataAddIn-Save.csv", header = TRUE)

dsdv <- subset(reachData, reachData$ProtocolType=="DSDV" & reachData$TTL=="NAN")
dsr0 <- subset(reachData, reachData$ProtocolType=="DSR" & reachData$TTL==0)
dsr1 <- subset(reachData, reachData$ProtocolType=="DSR" & reachData$TTL==1)
dsr2 <- subset(reachData, reachData$ProtocolType=="DSR" & reachData$TTL==2)

zrp0 <- subset(reachData, reachData$ProtocolType=="ZRP" & reachData$TTL==0)
zrp1 <- subset(reachData, reachData$ProtocolType=="ZRP" & reachData$TTL==1)
zrp2 <- subset(reachData, reachData$ProtocolType=="ZRP" & reachData$TTL==2)

sets <- list(dsdv,dsr0, dsr1, dsr2, zrp0, zrp1, zrp2)
setsNames <- c("DSDV","DSR0", "DSR1", "DSR2", "ZRP0", "ZRP1", "ZRP2")
cl <- rainbow(length(sets))

plot(NULL,NULL, xlim = c(2050,4000), ylim = c(20,100), xlab = "Size", ylab = "% Found", main = "Reachability in AddIn network")

for(i in 1:length(sets)){
  set <- sets[[i]]
  lines(set$NetworkSize, set$FoundPercentage, col= cl[i], lwd = 2)
}
legend("right", legend=setsNames,
       col=cl, lty=1, cex=0.8, lwd = 2)

# Reachbility in Cluster network
reachData <- read.csv("~/Dropbox/Thesis/Data/Logs/ReachClusterDataAllNew.csv", header = TRUE)

dsdv <- subset(reachData, reachData$ProtocolType=="DSDV" & reachData$TTL=="NAN")
dsr1 <- subset(reachData, reachData$ProtocolType=="DSR" & reachData$TTL==1)
dsr2 <- subset(reachData, reachData$ProtocolType=="DSR" & reachData$TTL==2)

zrp0 <- subset(reachData, reachData$ProtocolType=="ZRP" & reachData$TTL==0)
zrp1 <- subset(reachData, reachData$ProtocolType=="ZRP" & reachData$TTL==1)
zrp2 <- subset(reachData, reachData$ProtocolType=="ZRP" & reachData$TTL==2)

sets <- list(dsdv, dsr1, dsr2, zrp0, zrp1, zrp2)
setsNames <- c("DSDV", "DSR1", "DSR2", "ZRP0", "ZRP1", "ZRP2")
cl <- rainbow(length(sets))

plot(NULL,NULL, xlim = c(1850, 7000), ylim = c(20,100), xlab = "Size", ylab = "% Found", main = "Reachability in Cluster network")

for(i in 1:length(sets)){
  set <- sets[[i]]
  lines(set$NetworkSize, set$FoundPercentage, col= cl[i], lwd =2)
}
legend("right", legend=setsNames,
       col=cl, lty=1, cex=0.8, lwd =2)

# Reach for repeats

reachData <- read.csv("~/Dropbox/Thesis/Data/Logs/reachDataOriginal-Save.csv", header = TRUE)

dsr0 <- subset(reachData, reachData$ProtocolType=="DSR" & reachData$TTL==0)
dsr1 <- subset(reachData, reachData$ProtocolType=="DSR" & reachData$TTL==1)
dsr2 <- subset(reachData, reachData$ProtocolType=="DSR" & reachData$TTL==2)

zrp0 <- subset(reachData, reachData$ProtocolType=="ZRP" & reachData$TTL==0)
zrp1 <- subset(reachData, reachData$ProtocolType=="ZRP" & reachData$TTL==1)
zrp2 <- subset(reachData, reachData$ProtocolType=="ZRP" & reachData$TTL==2)

sets <- list(dsr0, dsr1, dsr2, zrp0, zrp1, zrp2)
setsNames <- c("DSR0", "DSR1", "DSR2", "ZRP0", "ZRP1", "ZRP2")
cl <- rainbow(length(sets))

plot(NULL,NULL, xlim = c(1,3), ylim = c(20,100), xlab = "Repeats", ylab = "% Found", main = "Reachability over repeats")

for(i in 1:length(sets)){
  set <- sets[[i]]
  lines(set$Repeats, set$FoundPercentage, col= cl[i], lwd = 2)
}
legend("right", legend=setsNames,
       col=cl, lty=1, cex=0.8, lwd = 2)

