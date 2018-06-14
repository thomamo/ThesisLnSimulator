using System;
using LnSim.Expanders;

namespace LnSim
{
    class Program
    {
        static void Main()
        {
            // table size tests
            //TestSuites.TestTableSize(new LnScraper(new OriginalExpander()), "TableSizesOriginal", "Original", "DSDV,DSR,ZRP,ZPRUNE", 4, 1, 4, 4);
            //TestSuites.TestTableSize(new LnScraper(new AddInExpander()), "TableSizesAddIn", "AddIn", "DSDV,DSR,ZRP,ZPRUNE", 5001, 500, 4, 4);
            //TestSuites.TestTableSize(new LnScraper(new ClusterExpander()), "TableSizesCluster", "Cluster", "DSDV,DSR,ZRP,ZPRUNE", 5, 1, 4, 4);

            // reachbility tests
            TestSuites.TestOfNetwork(new LnScraper(new ClusterExpander(), true), "ReachClusterDataAllNew", "Cluster", "DSDV,DSR,ZRP,ZPRUNE", 7, 1, 2, 3, true);

            // writing out visual data
            //UtilWriter.WriteGraphJson(new LnScraper(new ClusterExpander()), 2);

            // Code to create histogram data.
            //UtilWriter.WriteNodesLinksData(new LnScraper(new OriginalExpander()));
            //UtilWriter.WriteNodesLinksData(new LnScraper(new OriginalExpander()), "Original");
            //UtilWriter.WriteNodesLinksData(new LnScraper(new AddInExpander()), "AddIn500", 500);
            //UtilWriter.WriteNodesLinksData(new LnScraper(new AddInExpander()), "AddIn1500", 1500);
            //UtilWriter.WriteNodesLinksData(new LnScraper(new AddInExpander()), "AddIn2500", 2500);
            //UtilWriter.WriteNodesLinksData(new LnScraper(new ClusterExpander()), "Cluster2", 2);
            //UtilWriter.WriteNodesLinksData(new LnScraper(new ClusterExpander()), "Cluster4", 4);
            //UtilWriter.WriteNodesLinksData(new LnScraper(new ClusterExpander()), "Cluster6", 6);

            Console.WriteLine("Done");
        }
    }
}
