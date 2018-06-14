using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LnSim.Util
{
    public class UtilWriter
    {
        private const string Path = "../../../../../Data/";
        public static void WriteNodesLinksData(LnScraper scraper, string prefix, int size = 1)
        {
            var lnNodes = scraper.CreateLnComp(size);
            using (StreamWriter file = new StreamWriter(Path + prefix + "Hist/nodes.csv"))
            {
                lnNodes.ForEach(n => file.WriteLine($"{n.Id},{n.Connections.Count}"));
            }

            using (StreamWriter file = new StreamWriter(Path + prefix + "Hist/links.csv"))
            {
                lnNodes.ForEach(n => n.Connections.ForEach(c => file.WriteLine($"{n.Id},{c}")));
            }
        }

        public static void WriteGraphJson(LnScraper scraper, int size = 1)
        {
            var lnNodes = scraper.CreateLnComp(size);
            List<Node> nodes = new List<Node>();
            List<Link> links = new List<Link>();
            lnNodes.ForEach(n => nodes.Add(new Node()
            {
                id = n.Id,
                group = Int32.Parse(n.Id.Substring(n.Id.Length-1))
            }));

            lnNodes.ForEach(n =>
            {
                n.Connections.ForEach(c => links.Add(new Link() {source = n.Id, target = c, value = 1}));
            });

            var obj = new RootObject() {nodes = nodes, links = links};
            File.WriteAllText(Path + @"Visual/network.json", JsonConvert.SerializeObject(obj));
        }
    }

    public class Node
    {
        public string id { get; set; }
        public int group { get; set; }
    }

    public class Link
    {
        public string source { get; set; }
        public string target { get; set; }
        public int value { get; set; }
    }

    public class RootObject
    {
        public List<Node> nodes { get; set; }
        public List<Link> links { get; set; }
    }
}