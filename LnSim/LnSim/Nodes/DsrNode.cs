using System;
using System.Collections.Generic;
using System.Linq;

namespace LnSim.Nodes
{
    public class DsrNode : IProtocolNode
    {
        public List<Tuple<string, string, int>> RoutingTable { get; set; }
        public DsrNode(string id, List<string> connections)
        {
            Id = id;
            Connections = connections;
            RoutingTable = new List<Tuple<string, string, int>>();
        }

        public string Id { get; }
        public List<string> Connections { get; }
        public bool FindRouteTo(string id, List<IProtocolNode> network, int ttl = 10)
        {
            if (RoutingTable.Any(t => t.Item1.Contains(id)))
                return true;

            bool routeFound = false;
            
            for (int i = 1; i <= ttl; i++)
            {
                if (routeFound)
                    break;
                var before = RoutingTable.Count;
                var table = new List<Tuple<string, string, int>>(GetRoutingTable());

                foreach (var t in table)
                {
                    if (routeFound)
                        break;
                    if (t.Item3.Equals(i))
                    {
                        var node = network.Find(n => n.Id.Equals(t.Item1));
                        List<Tuple<string, string, int>> tempTable = new List<Tuple<string, string, int>>(node.GetRoutingTable());
                        if (tempTable.Any(r => r.Item1.Contains(id)))
                        {
                            routeFound = true;
                        }
                        else
                        {
                            AddNodeRt(node, i + 1);
                        }
                    }
                }

                if (before == RoutingTable.Count)
                    return routeFound;
            }

            return routeFound;
        }

        public List<Tuple<string, string, int>> GetRoutingTable()
        {
            List<Tuple<string, string, int>> tmp = new List<Tuple<string, string, int>>(RoutingTable.ToList());
            return tmp;
        }

        private void AddToTable(IProtocolNode node, int count)
        {
            node?.Connections.ForEach(c =>
            {
                if (!RoutingTable.Any(t => t.Item1.Contains(c)))
                {
                    RoutingTable.Add(new Tuple<string, string, int>(c, node.Id, count));
                }
            });


        }

        private void AddNodeRt(IProtocolNode node, int count)
        {
            if (node == null)
                return;
            
            List<Tuple<string, string, int>> tempTable = new List<Tuple<string, string, int>>(node.GetRoutingTable());
            if (tempTable.Count < 1)
                return;
            tempTable.ForEach(c =>
            {
                if (!RoutingTable.Any(t => t.Item1.Contains(c.Item1)))
                {
                    RoutingTable.Add(new Tuple<string, string, int>(c.Item1, node.Id, count));
                }
            });


        }

        public void Init(List<LnNode> lnNodes)
        {
            RoutingTable = new List<Tuple<string, string, int>>();
            RoutingTable.Add(new Tuple<string, string, int>(Id, Id, 0)); // Adds itself to its routing table
            Connections.ForEach(c => AddToTable(lnNodes.Find(node => node.Id.Equals(c)), 1)); // Adds connections to routing table
        }
    }
}