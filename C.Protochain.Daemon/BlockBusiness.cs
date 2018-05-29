using C.Protochain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using C.Protochain.Store;
using C.Protochain.Utils;

namespace C.Protochain.Daemon
{
    public static class BlockBusiness
    {
        //public static Block GenesisBlock = BuildGenesisBlock();
        //public static Block GenerateNextBlock(string data)
        //{
        //    Block latestBlock = GetLatestBlock();
        //    long nextIndex = latestBlock.Index + 1;
        //    Block nextBlock = new Block(nextIndex, data, latestBlock.Hash);

        //    return nextBlock;
        //}

        //internal static Block BuildGenesisBlock()
        //{
        //    Block genesis = new Block(1, "GENESIS", "901131D838B17AAC0F7885B81E03CBDC9F5157A00343D30AB22083685ED1416A");

        //    return genesis;
        //}

        //private static Block GetLatestBlock()
        //{
        //    throw new NotImplementedException();
        //}

        //public static bool ValidateBlock(Block newBlock, Block previousBlock)
        //{
        //    // Check index
        //    if (newBlock.Index != previousBlock.Index + 1)
        //        return false;

        //    // Check chain
        //    if (newBlock.PreviousHash != previousBlock.Hash)
        //        return false;

        //    // Check hash
        //    if (StringExtensions.ConstructHashable(newBlock.Index, newBlock.Data, newBlock.PreviousHash, newBlock.Timestamp, newBlock.Nonce).ComputeSha256Hash() != newBlock.Hash)
        //        return false;

        //    return true;
        //}

        //public static bool ValidateChain(Block[] chain)
        //{
        //    // Validate that the first block stored is the chain genesis block
        //    bool isValidGenesis = DataStore.GetByIndex(1).Data == GenesisBlock.Data;
        //    if (!isValidGenesis)
        //        return false;

        //    for (int i = 1; i < chain.Length; i++)
        //    {
        //        // Validate each block of the chain
        //        if (!ValidateBlock(chain[i], chain[i - 1]))
        //            return false;
        //    }

        //    // Healthy chain
        //    return true;
        //}

        //public static void ReplaceChain(Block[] newChain)
        //{
        //    // Choosing the longuest chain
        //    var isValidChain = ValidateChain(newChain);
        //    var newChainIsLongest = newChain.Length > DataStore.GetBlockchain().Length;

        //    if (isValidChain && newChainIsLongest)
        //    {
        //        DataStore.ReplaceBlockchain(newChain);
        //        // BroadcastNewChain();
        //    }
        //}

    }
}
