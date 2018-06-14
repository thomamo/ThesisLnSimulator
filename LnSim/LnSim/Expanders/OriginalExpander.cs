﻿using System.Collections.Generic;
using LnSim.Nodes;

namespace LnSim.Expanders
{
    public class OriginalExpander : ILnExpander
    {
        public List<LnNode> ExpandNetwork(List<LnNode> network, int size = 0, int param = 0)
        {
            return network;
        }
    }
}