using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace LnScraper
{
    public static class LnScraper
    {
        public static List<LnNode> ScrapeLnComposition()
        {
            List<LnNode> lnNodes = new List<LnNode>();

            using (StreamReader r = new StreamReader("../../../../../Data/nodes.json"))
            {
                string json = r.ReadToEnd();
                //List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
                JObject rss = JObject.Parse(json);

                var nodes = rss["nodes"];

                foreach (var nodeToken in nodes)
                {
                    lnNodes.Add(new LnNode()
                    {
                        Id = nodeToken["nodeid"].ToString(),
                        Connections = new List<string>()
                    });
                }
            }

            using (StreamReader r = new StreamReader("../../../../../Data/channels.json"))
            {
                string json = r.ReadToEnd();
                //List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
                JObject rss = JObject.Parse(json);

                var channels = rss["channels"];

                foreach (var channelToken in channels)
                {
                    if ((bool)channelToken["active"])
                    {
                        lnNodes.Find(n => n.Id.Equals(channelToken["source"].ToString())).Connections.Add(channelToken["destination"].ToString());
                    }
                }
            }

            Console.WriteLine(lnNodes.Count);

            lnNodes.RemoveAll(n => n.Connections.Count == 0);

            Console.WriteLine(lnNodes.Count);
            

            return lnNodes;
        }
    }
}
