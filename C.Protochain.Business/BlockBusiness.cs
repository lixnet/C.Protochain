using System;
using System.Linq;
using System.Runtime.CompilerServices;
using C.Protochain.Entities;
using C.Protochain.Store;
using C.Protochain.Utils;

namespace C.Protochain.Business
{
    public static class BlockBusiness
    {
        public static Block GenesisBlock;
        public const int BlockGenerationInterval = 10;
        public const int DifficultyAdjustmentInterval = 10;

        public static Block GenerateNextBlock(string data)
        {
            Block latestBlock = GetLatestBlock();
            long nextIndex = latestBlock.Index + 1;
            Block nextBlock = new Block(nextIndex, data, latestBlock.Hash, GetDifficulty());
            if (ValidateBlock(nextBlock, GetLatestBlock()))
                DataStore.PushToBlockchain(nextBlock);

            return nextBlock;
        }

        public static void BuildGenesisBlock()
        {
            Block genesis = new Block(0, "GENESIS", "901131D838B17AAC0F7885B81E03CBDC9F5157A00343D30AB22083685ED1416A", 1);
            GenesisBlock = genesis;
            DataStore.PushToBlockchain(genesis);
        }
        private static int GetDifficulty()
        {
            var lastBlock = GetLatestBlock();
            if (lastBlock.Index % DifficultyAdjustmentInterval == 0 && lastBlock.Index != 0)
            {
                return GetNewDifficulty();
            }

            return lastBlock.Difficulty;
        }

        private static int GetNewDifficulty()
        {
            Block[] blockchain = GetBlockchain();
            Block lastBlock = blockchain.Last();
            Block lastAdjustedBlock = blockchain[blockchain.Length - DifficultyAdjustmentInterval];
            long expectedTimeBetweenBlocks = BlockGenerationInterval * DifficultyAdjustmentInterval;
            double actualTimeBetweenBlocks = TimeSpan.FromTicks(lastBlock.Timestamp - lastAdjustedBlock.Timestamp).TotalSeconds;
            if (actualTimeBetweenBlocks < expectedTimeBetweenBlocks / 2d)
                return lastAdjustedBlock.Difficulty + 1;

            if (actualTimeBetweenBlocks > expectedTimeBetweenBlocks * 2d)
                return lastAdjustedBlock.Difficulty - 1;

            return lastAdjustedBlock.Difficulty;
        }

        private static Block GetLatestBlock()
        {
            return DataStore.GetLastBlock();
        }

        private static Block[] GetBlockchain()
        {
            return DataStore.GetBlockchain();
        }

        public static bool ValidateBlock(Block newBlock, Block previousBlock)
        {
            // Check index
            if (newBlock.Index != previousBlock.Index + 1)
                return false;

            // Check chain
            if (newBlock.PreviousHash != previousBlock.Hash)
                return false;

            // Check timestamp
            var validStamp = TimeSpan.FromTicks(previousBlock.Timestamp).TotalSeconds - 60 <
                             TimeSpan.FromTicks(newBlock.Timestamp).TotalSeconds
                             &&
                             TimeSpan.FromTicks(newBlock.Timestamp).TotalSeconds - 60 <
                             TimeSpan.FromTicks(DateTime.UtcNow.Ticks).TotalSeconds;

            if (!validStamp)
                return false;
                
            // Check hash
            var newHash = StringExtensions.ConstructHashable(newBlock.Index, newBlock.Data, newBlock.PreviousHash, newBlock.Timestamp, newBlock.Nonce).ComputeSha256Hash();
            if (newHash != newBlock.Hash)
                return false;

            return true;
        }

        public static bool ValidateChain(Block[] chain)
        {
            // Validate that the first block stored is the chain genesis block
            bool isValidGenesis = DataStore.GetByIndex(0).Data == GenesisBlock.Data;
            if (!isValidGenesis)
                return false;

            for (int i = 1; i < chain.Length; i++)
            {
                // Validate each block of the chain
                if (!ValidateBlock(chain[i], chain[i - 1]))
                    return false;
            }

            // Healthy chain
            return true;
        }

        public static void ReplaceChain(Block[] newChain)
        {
            // Choosing the longuest chain
            var isValidChain = ValidateChain(newChain);
            var currentChain = GetBlockchain();
            
            var newChainCumulativeDifficulty = newChain.Sum(_ => Math.Pow(_.Difficulty, 2));
            var currentChainCumulativeDifficulty = currentChain.Sum(_ => Math.Pow(_.Difficulty, 2));

            if (isValidChain && (newChainCumulativeDifficulty > currentChainCumulativeDifficulty))
            {                
                // BroadcastNewChain();
            }
        }
    }
}
