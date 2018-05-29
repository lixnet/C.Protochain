using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C.Protochain.Entities;

namespace C.Protochain.Store
{
    public static class DataStore
    {
        // Memory set
        public static Block[] GetBlockchain()
        {
            return Memory.Blockchain.Values.ToArray();
        }

        public static Block GetLastBlock()
        {
            return Memory.Blockchain.Values.ToArray().Last();
        }

        public static void PushToBlockchain(Block block)
        {
            Memory.Blockchain.Add(block.Index, block);
        }

        public static Block GetByIndex(long index)
        {
            return Memory.Blockchain[index];
        }

        public static void ReplaceBlockchain(Block[] newChain)
        {
            var tempStore = new SortedDictionary<long, Block>();
            for (int i = 0; i < newChain.Length; i++)
            {
                tempStore.Add(newChain[i].Index, newChain[i]);
            }

            Memory.Blockchain = tempStore;            
        }
    }
}
