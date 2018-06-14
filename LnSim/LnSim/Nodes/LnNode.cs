using System;
using System.Collections.Generic;

namespace LnSim.Nodes
{
    public class LnNode : IProtocolNode
    {
        public LnNode(string id, List<string> connections)
        {
            Id = id;
            Connections = connections;
        }

        public string Id { get; set; }
        public List<string> Connections { get; set; }
        public List<Tuple<string, string, int>> RoutingTable { get; set; }

        public bool FindRouteTo(string id, List<IProtocolNode> network = null, int ttl = 0)
        {
            throw new NotImplementedException();
        }

        public List<Tuple<string, string, int>> GetRoutingTable()
        {
            throw new NotImplementedException();
        }

        public void Init(List<LnNode> lnNodes)
        {
            throw new NotImplementedException();
        }
    }
}