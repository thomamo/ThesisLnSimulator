using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using LnSim.Expanders;
using LnSim.Nodes;
using Newtonsoft.Json.Linq;

namespace LnSim
{
    public class LnScraper
    {
        private readonly ILnExpander _expander;
        private const string DataPath = "../../../../../Data/LnData/";
        private const string NodesUrl = "http://shabang.io/nodes.json";
        private const string ChannelsUrl = "http://shabang.io/channels.json";

        private readonly string _nodesPath = $"{DataPath}nodes{DateTime.Today:yyyy-MM-dd}.json";
        private readonly string _channelsPath = $"{DataPath}channels{DateTime.Today:yyyy-MM-dd}.json";

        public LnScraper(ILnExpander expander, bool takeFromFile = false)
        {
            _expander = expander;

            if (takeFromFile) // Since shabang.io has not been avaivable at times, reading from file is an option
            {
                _nodesPath = $"{DataPath}nodes.json";
                _channelsPath = $"{DataPath}channels.json";

            }
            else
            {
                if (!File.Exists(_nodesPath) && !File.Exists(_channelsPath))
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(NodesUrl, _nodesPath);
                        client.DownloadFile(ChannelsUrl, _channelsPath);
                    }
            }
            
        }

        public List<LnNode> CreateLnComp(int size = 0, int param = 1)
        {
            var lnNodes = new List<LnNode>();

            using (var r = new StreamReader(_nodesPath))
            {
                var json = r.ReadToEnd();
                var rss = JObject.Parse(json);

                var nodes = rss["nodes"];

                foreach (var nodeToken in nodes)
                    lnNodes.Add(new LnNode(nodeToken["nodeid"].ToString(), new List<string>()));
            }

            using (var r = new StreamReader(_channelsPath))
            {
                var json = r.ReadToEnd();
                var rss = JObject.Parse(json);

                var channels = rss["channels"];

                foreach (var channelToken in channels)
                    if (true) //(bool)channelToken["active"])
                        lnNodes.Find(n => n.Id.Equals(channelToken["source"].ToString())).Connections
                            .Add(channelToken["destination"].ToString());
            }

            Console.WriteLine($"Original size: {lnNodes.Count}");
            var expandedNodes = new List<LnNode>(_expander.ExpandNetwork(lnNodes, size, param));
            Console.WriteLine($"Expanded size: {expandedNodes.Count}");
            Console.WriteLine($"Nodes with no channels: {expandedNodes.Count(n => n.Connections.Count < 1)}");

            return expandedNodes;
        }
    }
}