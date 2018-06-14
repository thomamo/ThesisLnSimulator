using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LnSim.Nodes;
using LnSim.Util;

namespace LnSim
{
    public class TestSuites
    {
        public static double TestReachability(List<IProtocolNode> nodes, bool multi, int ttl = 0, int repeats = 1)
        {
            double successfulRoutes = 0;
            Random rnd = new Random();

            if (multi)
            {
                Parallel.ForEach(nodes, n =>
                    {
                        for (int i = 0; i < repeats; i++)
                        {
                            if (n.FindRouteTo(nodes[rnd.Next(nodes.Count)].Id, nodes, ttl))
                            {
                                successfulRoutes++;
                            }
                        }
                    }); 
            }
            else
            {
                nodes.ForEach(n =>
                {
                    for (int i = 0; i < repeats; i++)
                    {
                        if (n.FindRouteTo(nodes[rnd.Next(nodes.Count)].Id, nodes, ttl))
                        {
                            successfulRoutes++;
                        }
                    }
                });
            }

            Console.WriteLine($"{successfulRoutes} out out {nodes.Count*repeats}");
            double res = (successfulRoutes / (nodes.Count * repeats)) * 100d;
            Console.WriteLine($"TTL={ttl}, Repeats={repeats}, Found: {res}%");
            return res;
        }

        public static double FindRoutesSubset(List<IProtocolNode> nodes, bool multi, List<int> subset, int ttl = 0)
        {
            Random rnd = new Random();
            var testNodes = new List<IProtocolNode>();
            subset.ForEach(index => testNodes.Add(nodes[index]));
            var successfulRoutes = 0;
            if (multi)
            {
                Parallel.ForEach(testNodes, n =>
                {
                    if (n.FindRouteTo(nodes[rnd.Next(nodes.Count)].Id, nodes, ttl))
                    {
                        successfulRoutes++;
                    } 

                });
            }
            else
            {
                testNodes.ForEach(n =>
                {
                    if (n.FindRouteTo(nodes[rnd.Next(nodes.Count)].Id, nodes, ttl))
                    {
                        successfulRoutes++;
                    }
                });
            }

            double res = (double)successfulRoutes / (testNodes.Count) * 100d;
            return res;
        }

        private static double AvgTableSize(List<IProtocolNode> nodes, List<int> indexes, int ttl, bool multi = true)
        {
            int tmp = 0;
            FindRoutesSubset(nodes, multi, indexes, ttl);
            indexes.ForEach(i =>
            {
                tmp += nodes[i].RoutingTable.Count;
            });

            return (double)tmp / indexes.Count;
        }

        public static void TestTableSize(LnScraper scraper, string fileName, string networkType, string protocols, int size, int stepSize, int repeats, int ttl)
        {
            using (var logger = new CsvLogger(fileName, true,
                "ProtocolType,NetworkType,NetworkSize,TTL,Repeats,Size,TableSize"))
            {
                for (int i = 1; i < size; i = i + stepSize)
                {
                    var testNodes = scraper.CreateLnComp(i);
                    int networkSize = testNodes.Count;

                    // creates list of the indexes of the nodes we check. We check the same node in each protocol
                    int subsetCount = (int)(networkSize * (2 / 100d)); // Test on 2% of the nodes
                    Random rnd = new Random();
                    List<int> rndIndexes = new List<int>();
                    
                    for (int j = 0; j < subsetCount; j++)
                    {
                        rndIndexes.Add(rnd.Next(networkSize - 1));
                    }

                    // dsdv test
                    if (protocols.Contains("DSDV"))
                    {
                        var dsdvNodes = CompositionBuilder.CreateDsdvNodes(testNodes, rndIndexes);
                        for (int reps = 1; reps < repeats; reps++)
                        {
                            double res = AvgTableSize(dsdvNodes, rndIndexes, 1);
                            logger.WriteLine($"DSDV,{networkType},{networkSize},NAN,{reps},{i},{res}");
                        }
                    }

                    //dsr test
                    if (protocols.Contains("DSR"))
                    {
                        var dsrNodes = CompositionBuilder.CreateDsrNodes(testNodes, rndIndexes);
                        for (int j = 0; j < ttl; j++)
                        {
                            for (int reps = 1; reps < repeats; reps++)
                            {
                                double res = AvgTableSize(dsrNodes, rndIndexes, 1, false);
                                logger.WriteLine($"DSR,{networkType},{networkSize},{j},{reps},{i},{res}");
                            }
                        }
                    }

                    // zrp test
                    if (protocols.Contains("ZRP"))
                    {
                        var zrpNodes = CompositionBuilder.CreateZrpNodes(testNodes, rndIndexes);
                        for (int j = 0; j < ttl; j++)
                        {
                            for (int reps = 1; reps < repeats; reps++)
                            {
                                double res = AvgTableSize(zrpNodes, rndIndexes, 1);
                                logger.WriteLine($"ZRP,{networkType},{networkSize},{j},{reps},{i},{res}");
                            }
                        }
                    }

                    // zrp prune test
                    if (protocols.Contains("ZPRUNE"))
                    {
                        var zrpNodes = CompositionBuilder.CreateZrpNodes(testNodes, rndIndexes, true);
                        for (int j = 0; j < ttl; j++)
                        {
                            for (int reps = 1; reps < repeats; reps++)
                            {
                                double res = AvgTableSize(zrpNodes, rndIndexes, 1);
                                logger.WriteLine($"ZPRUNE,{networkType},{networkSize},{j},{reps},{i},{res}");
                            }
                        }
                    }
                }
            }
        }

        public static void TestOfNetwork(LnScraper scraper, string fileName, string networkType, string protocols, int size, int stepSize, int repeats, int ttl, bool subsetting)
        {
            using (var logger = new CsvLogger(fileName, true,
                "ProtocolType,NetworkType,NetworkSize,NoOutboundChannels,TTL,Repeats,Size,FoundPercentage"))
            {

                for (int i = 1; i < size; i = i + stepSize)
                {
                    try
                    {
                        var testNodes = scraper.CreateLnComp(i);
                        double noOutboundChannels = testNodes.Count(n => n.Connections.Count < 1);
                        int networkSize = testNodes.Count;

                        // dsdv test
                        if (protocols.Contains("DSDV"))
                        {
                            Random rnd = new Random();
                            List<int> rndIndexes = new List<int>();

                            for (int j = 0; j < 200; j++)
                            {
                                int r = rnd.Next(networkSize - 1);
                                if (rndIndexes.Contains(r))
                                    j--;
                                else
                                    rndIndexes.Add(r);
                            }

                            var dsdvNodes = CompositionBuilder.CreateDsdvNodes(testNodes, rndIndexes);
                            for (int reps = 1; reps < repeats; reps++)
                            {
                                var found = FindRoutesSubset(dsdvNodes, true, rndIndexes);
                                logger.WriteLine($"DSDV,{networkType},{networkSize},{noOutboundChannels},NAN,{reps},{i},{found}");
                            }
                        }

                        //dsr test
                        if (protocols.Contains("DSR"))
                        {
                            if (!subsetting)
                            {
                                var dsrNodes = CompositionBuilder.CreateDsrNodes(testNodes);
                                for (int j = 0; j < ttl; j++)
                                {
                                    for (int reps = 1; reps < repeats; reps++)
                                    {
                                        var found = TestReachability(dsrNodes, false, j, reps);
                                        logger.WriteLine($"DSR,{networkType},{networkSize},{noOutboundChannels},{j},{reps},{i},{found}");
                                    }
                                } 
                            }
                            else
                            {
                                Random rnd = new Random();
                                List<int> rndIndexes = new List<int>();

                                for (int j = 0; j < 1000; j++)
                                {
                                    int r = rnd.Next(networkSize - 1);
                                    if (rndIndexes.Contains(r))
                                        j--;
                                    else
                                        rndIndexes.Add(r);
                                }

                                var dsrNodes = CompositionBuilder.CreateDsrNodes(testNodes, rndIndexes);
                                for (int j = 0; j < ttl; j++)
                                {
                                    for (int reps = 1; reps < repeats; reps++)
                                    {
                                        var found = FindRoutesSubset(dsrNodes, false, rndIndexes, j);
                                        logger.WriteLine($"DSR,{networkType},{networkSize},{noOutboundChannels},{j},{reps},{i},{found}");
                                    }
                                }
                            }
                        }

                        // zrp test
                        if (protocols.Contains("ZRP"))
                        {
                            var zrpNodes = CompositionBuilder.CreateZrpNodes(testNodes);
                            for (int j = 0; j < ttl; j++)
                            {
                                for (int reps = 1; reps < repeats; reps++)
                                {
                                    var found = TestReachability(zrpNodes, true, j, reps);
                                    logger.WriteLine($"ZRP,{networkType},{networkSize},{noOutboundChannels},{j},{reps},{i},{found}");
                                }
                            }
                        }

                        // zrp test
                        if (protocols.Contains("ZPRUNE"))
                        {
                            var zrpNodes = CompositionBuilder.CreateZrpNodes(testNodes, null, true);
                            for (int j = 0; j < ttl; j++)
                            {
                                for (int reps = 1; reps < repeats; reps++)
                                {
                                    var found = TestReachability(zrpNodes, true, j, reps);
                                    logger.WriteLine($"ZPRUNE,{networkType},{networkSize},{noOutboundChannels},{j},{reps},{i},{found}");
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        if (i > 1)
                            i-= stepSize;
                    }
                }
            }
        }
    }
}