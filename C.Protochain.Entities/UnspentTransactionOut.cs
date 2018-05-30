using System;
using System.Collections.Generic;
using System.Text;

namespace C.Protochain.Entities
{
    public class UnspentTransactionOut
    {
        public string TransactionOutId { get; }
        public long TransactionOutIndex { get; }
        public string Address { get; }
        public long Amount { get; }

        public UnspentTransactionOut(string transactionOutId, long transactionOutIndex, string address, long amount)
        {
            TransactionOutId = transactionOutId;
            TransactionOutIndex = transactionOutIndex;
            Address = address;
            Amount = amount;
        }
    }
}
