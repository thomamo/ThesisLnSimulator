using System;
using System.Collections.Generic;
using System.Linq;

namespace LnSim.Nodes
{
    public class ZrpNode : IProtocolNode
    {
        public ZrpNode(string id, List<string> connections, bool prune = false)
        {
            Id = id;
            Connections = connections;
            RoutingTable = new List<Tuple<string, string, int>>();
            Prune = prune;
        }

        public bool Prune { get; set; }
        public string Id { get; }
        public List<string> Connections { get; }
        public List<Tuple<string, string, int>> RoutingTable { get; set; }

        public void Init(List<LnNode> network)
        {
            RoutingTable.Add(new Tuple<string, string, int>(Id, Id, 0)); // Adds itself to its routing table
            Connections.ForEach(c =>
                AddToTable(network.Find(node => node.Id.Equals(c)), 1)); // Adds connections to routing table

            var table = new List<Tuple<string, string, int>>(RoutingTable);
            table.ForEach(t => AddToTable(network.Find(node => node.Id.Equals(t.Item1)), 2)); // Adding peripheral nodes

            Connections.ForEach(c => RoutingTable.RemoveAll(r => c.Equals(r.Item1))); // Removing connections and itself
            RoutingTable.RemoveAll(r => r.Item1.Equals(Id));

            if (Prune)
            {
                var tmp = RoutingTable.OrderBy(r => network.Find(node => node.Id.Equals(r.Item1)).Connections.Count);
                if (tmp.Count() >= Connections.Count + 10)
                {
                    RoutingTable = tmp.TakeLast(Connections.Count + 10).ToList();
                }
            }
        }

        public bool FindRouteTo(string id, List<IProtocolNode> network = null, int ttl = 1)
        {
            if (network == null) return false;

            if (Connections.Any(c => c.Contains(id)) || RoutingTable.Any(r => r.Item1.Contains(id))) return true;

            if (ttl == 0) return false;

            var dic = new Dictionary<string, int>();
            Connections.ForEach(c => dic.Add(c, 0));
            RoutingTable.ForEach(r => dic.Add(r.Item1, 0));
            dic.Add(Id, 0);

            var routeFound = false;
            RoutingTable.ForEach(p =>
            {
                var node = network.Find(nnode => nnode.Id.Equals(id));
                if (node.Connections.Any(c => c.Contains(id)) || node.RoutingTable.Any(r => r.Item1.Contains(id)))
                    routeFound = true;
                else
                    node.RoutingTable.ForEach(r =>
                    {
                        if (!dic.ContainsKey(r.Item1))
                            dic.Add(r.Item1, 1);
                    });
            });

            if (dic.ContainsKey(id))
            {
                routeFound = true;
            }

            for (var i = 2; i <= ttl; i++)
            {
                if (routeFound)
                    break;

                var tmpDic = new Dictionary<string, int>(dic);
                foreach (var keyValuePair in tmpDic)
                    if (keyValuePair.Value == i - 1)
                    {
                        if (routeFound)
                            break;

                        var node = network.Find(nnode => nnode.Id.Equals(keyValuePair.Key));
                        if (node.Connections.Any(c => c.Contains(id)) ||
                            node.RoutingTable.Any(r => r.Item1.Contains(id)))
                            routeFound = true;
                        else
                            node.RoutingTable.ForEach(r =>
                            {
                                if (!dic.ContainsKey(r.Item1))
                                    dic.Add(r.Item1, i);
                            });
                    }
            }

            return routeFound;
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