using System;
using System.Collections.Generic;
using System.Linq;

namespace LnSim.Nodes
{
    public class DsdvNode : IProtocolNode
    {
        public DsdvNode(string id, List<string> connections)
        {
            Id = id;
            Connections = connections;
            RoutingTable = new List<Tuple<string, string, int>>();
        }

        public string Id { get; }
        public List<string> Connections { get; }
        public List<Tuple<string, string, int>> RoutingTable { get; set; }

        public void Init(List<LnNode> network)
        {
            RoutingTable.Add(new Tuple<string, string, int>(Id, Id, 0)); // Adds itself to its routing table
            Connections.ForEach(c =>
                AddToTable(network.Find(node => node.Id.Equals(c)), 1)); // Adds connections to routing table

            for (var i = 2; i < 15; i++)
            {
                var before = RoutingTable.Count;
                var table = new List<Tuple<string, string, int>>(RoutingTable);
                foreach (var t in table.Where(r => r.Item3 == i-1))
                {
                    AddToTable(network.Find(node => node.Id.Equals(t.Item1)), i);
                }
                if (before == RoutingTable.Count) break;
            }
        }

        public bool FindRouteTo(string id, List<IProtocolNode> network = null, int ttl = 0)
        {
            return RoutingTable.Any(d => d.Item1.Contains(id));
        }

        public List<Tuple<string, string, int>> GetRoutingTable()
        {
            throw new NotImplementedException();
        }

        private void AddToTable(LnNode node, int count)
        {
            node?.Connections.ForEach(c =>
            {
                if (!RoutingTable.Any(t => t.Item1.Contains(c)))
                    RoutingTable.Add(new Tuple<string, string, int>(c, node.Id, count));
            });
        }
    }
}