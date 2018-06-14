using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LnSim.Nodes;

namespace LnSim
{
    public static class CompositionBuilder
    {
        public static List<IProtocolNode> CreateNodesOfType<T>(Type type, List<LnNode> lnNodes) where T : IProtocolNode
        {
            var ls = new List<IProtocolNode>();

            lnNodes.ForEach(n => ls.Add((T)Activator.CreateInstance(typeof(T), n.Id, n.Connections)));

            Parallel.ForEach(ls, n =>
            {
                n.Init(lnNodes);
            });
            return ls;
        }

        public static List<IProtocolNode> CreateZrpNodes(List<LnNode> lnNodes, List<int> list = null, bool prune = false)
        {
            var ls = new List<IProtocolNode>();

            lnNodes.ForEach(n => ls.Add(new ZrpNode(n.Id, n.Connections, prune)));

            if (list == null)
            {
                Parallel.ForEach(ls, n =>
                    {
                        n.Init(lnNodes);
                    }); 
            }
            else
            {
                Parallel.ForEach(list, index =>
                {
                    ls[index].Init(lnNodes);
                });
            }
            return ls;
        }

        public static List<IProtocolNode> CreateDsdvNodes(List<LnNode> lnNodes, List<int> list = null)
        {
            var ls = new List<IProtocolNode>();

            lnNodes.ForEach(n => ls.Add(new DsdvNode(n.Id, n.Connections)));

            if (list == null)
            {
                Parallel.ForEach(ls, n =>
                    {
                        n.Init(lnNodes);
                    }); 
            }
            else
            {
                list.ForEach(index =>
                {
                    ls[index].Init(lnNodes);
                });
            }
            return ls;
        }

        public static List<IProtocolNode> CreateDsrNodes(List<LnNode> lnNodes, List<int> list = null)
        {
            var ls = new List<IProtocolNode>();

            lnNodes.ForEach(n => ls.Add(new DsrNode(n.Id, n.Connections)));

            if (list == null)
            {
                Parallel.ForEach(ls, n =>
                    {
                        n.Init(lnNodes);
                    }); 
            }
            else
            {
                Parallel.ForEach(list, index =>
                {
                    ls[index].Init(lnNodes);
                });
            }
            return ls;
        }
    }
}