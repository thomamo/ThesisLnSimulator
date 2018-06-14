# Freq Hist plot original
nodes <- read.csv("~/Dropbox/Thesis/Data/Hist/Originalnodes.csv", header = FALSE)

cons <- nodes[[2]]
cons <- cons[cons != 0]
cons[cons > 20] <- 20

summary(cons)

h <- hist(cons, plot = FALSE, breaks = c(1:20))
plot(h, xaxt = "n", xlab = "Connections", main = "", col = "green", border = "blue", freq = TRUE)
axis(1, h$mids, labels = c(1:18, "20<"))

# AddIn vs Original Hist
nodes <- read.csv("~/Dropbox/Thesis/Data/Hist/Originalnodes.csv", header = FALSE)
cons <- nodes[[2]]
cons <- cons[cons != 0]
cons[cons > 20] <- 20
h <- hist(cons, plot = FALSE, breaks = c(1:20))
plot(h, xaxt = "n", xlab = "Connections", main = "", col = rgb(0,1,0,0.5), border = "blue", freq = FALSE)
axis(1, h$mids, labels = c(1:18, "20<"))

nodes <- read.csv("~/Dropbox/Thesis/Data/Hist/AddIn1500nodes.csv", header = FALSE)
cons <- nodes[[2]]
cons <- cons[cons != 0]
cons[cons > 20] <- 20
h <- hist(cons, plot = FALSE, breaks = c(1:20))
plot(h, col = rgb(1,0,0,0.5), border = "blue", freq = FALSE, add = TRUE)

legend("right", legend=c("Original", "AddIn1500"), title = "Expansion set", fill=c(rgb(0,1,0,0.5), rgb(1,0,0,0.5)), cex=0.8, inset=.04)

# Cluster Hist
par(mfrow=c(1,3))
nodes <- read.csv("~/Dropbox/Thesis/Data/Hist/Originalnodes.csv", header = FALSE)
cons <- nodes[[2]]
cons <- cons[cons != 0]
cons[cons > 20] <- 20
h <- hist(cons, plot = FALSE, breaks = c(1:20))
plot(h, xaxt = "n", xlab = "", main = "Original", col = rgb(0,1,0,0.5), border = "blue", freq = T)
axis(1, h$mids, labels = c(1:18, "20<"))

nodes <- read.csv("~/Dropbox/Thesis/Data/Hist/Cluster2nodes.csv", header = FALSE)
cons <- nodes[[2]]
cons <- cons[cons != 0]
cons[cons > 20] <- 20
h <- hist(cons, plot = FALSE, breaks = c(1:20))
plot(h, xaxt = "n", xlab = "", ylab = "", main = "2 Clusters", col = rgb(0,0,1,0.5), border = "blue", freq = T)
axis(1, h$mids, labels = c(1:18, "20<"))

nodes <- read.csv("~/Dropbox/Thesis/Data/Hist/Cluster4nodes.csv", header = FALSE)
cons <- nodes[[2]]
cons <- cons[cons != 0]
cons[cons > 20] <- 20
h <- hist(cons, plot = FALSE, breaks = c(1:20))
plot(h, xaxt = "n", xlab = "", ylab = "", main = "4 Clusters", col = rgb(1,0,0,0.5), border = "blue", freq = T)
axis(1, h$mids, labels = c(1:18, "20<"))

mtext("Connections", side=1, padj=-2, outer=TRUE)
