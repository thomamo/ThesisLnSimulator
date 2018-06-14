using System;
using System.Collections.Generic;

namespace LnSim.Nodes
{
    public interface IProtocolNode
    {
        string Id { get; }
        List<string> Connections { get; }
        List<Tuple<string, string, int>> RoutingTable { get; set; }
        void Init(List<LnNode> lnNodes);
        bool FindRouteTo(string id, List<IProtocolNode> network = null, int ttl = 0);
        List<Tuple<string, string, int>> GetRoutingTable();

    }
}