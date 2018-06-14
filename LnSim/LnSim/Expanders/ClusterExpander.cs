using System;
using System.Collections.Generic;
using System.Linq;
using LnSim.Nodes;

namespace LnSim.Expanders
{
    public class ClusterExpander : ILnExpander
    {
        public List<LnNode> ExpandNetwork(List<LnNode> network, int size = 1, int param = 1)
        {
            List<LnNode> res = new List<LnNode>();
            Random rnd = new Random();
            for (int i = 0; i < size; i++)
            {
                List<LnNode> tmp = new List<LnNode>();
                network.ForEach(n => tmp.Add(new LnNode(n.Id, n.Connections)));
                tmp.ForEach(n =>
                {
                    n.Id = $"{n.Id}C{i}";
                    List<string> nCons = new List<string>();
                    n.Connections.ForEach(c =>
                    {
                        nCons.Add($"{c}C{i}");
                    });
                    n.Connections = nCons;
                });

                int connectionsNeeded = (int)(res.Count * (param / 100d));

                for (int j = 0; j < connectionsNeeded; j++)
                {
                    LnNode netNode = res[rnd.Next(res.Count)];
                    LnNode clusterNode = tmp[rnd.Next(tmp.Count)];

                    if (netNode.Connections.Contains(clusterNode.Id))
                    {
                        j--;
                    }
                    else
                    {
                        netNode.Connections.Add(clusterNode.Id);
                        clusterNode.Connections.Add(netNode.Id);
                    }
                }

                res = res.Concat(tmp).ToList();
            }

            return res;
        }
    }
}