using C.Protochain.Utils;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace C.Protochain.Entities
{
    public class Block
    {
        public Block(long index, string data, string previousHash, int difficulty)
        {
            this.Index = index;
            this.Data = data;
            this.Timestamp = DateTime.UtcNow.Ticks;
            this.PreviousHash = previousHash;
            this.Difficulty = difficulty;
            this.Hash = Mine();            
        }
        public int Difficulty { get; private set; }
        public long Index { get; private set; }
        public string PreviousHash { get; }
        public string Hash { get; }
        public long Timestamp { get; }
        public string Data { get; }

        public long Nonce = -1;

        public string Mine()
        {            
            string hash = string.Empty;
            string beginsWith =  string.Join(string.Empty, Enumerable.Range(0, this.Difficulty).Select(_ => "0"));
            Console.WriteLine($"Mining block with difficulty of {Difficulty}");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (!hash.StartsWith(beginsWith))
            {
                Nonce++;
                hash = StringExtensions.ConstructHashable(Index, Data, PreviousHash, Timestamp, Nonce).ComputeSha256Hash();
            }
            sw.Stop();
            Console.WriteLine($"Mined block in {sw.ElapsedMilliseconds / 1000d}s");

            return hash;
        }
    }
}
