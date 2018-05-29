using System;
using System.Collections.Generic;
using C.Protochain.Entities;

namespace C.Protochain.Store
{
    public static class Memory
    {
        public static SortedDictionary<long, Block> Blockchain = new SortedDictionary<long, Block>();
    }
}
