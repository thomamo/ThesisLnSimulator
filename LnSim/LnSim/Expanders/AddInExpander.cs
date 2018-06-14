using System;
using System.Collections.Generic;
using LnSim.Nodes;

namespace LnSim.Expanders
{
    public class AddInExpander : ILnExpander
    {
        public List<LnNode> ExpandNetwork(List<LnNode> network, int size = 0, int param = 0)
        {
            var res = new List<LnNode>(network);
            Random rnd = new Random();

            for (int i = 0; i < size; i++)
            {
                var aLike = network[rnd.Next(network.Count)];

                List<string> newCons = new List<string>();
                string newId = $"{aLike.Id}N{i}";

                while (newCons.Count < aLike.Connections.Count)
                {
                    var index = rnd.Next(res.Count);
                    if (!newCons.Contains(res[index].Id))
                    {
                        newCons.Add(res[index].Id);
                        res[index].Connections.Add(newId);
                    }
                }
                
                res.Add(new LnNode(newId, newCons));
            }

            return res;
        }
    }
}