using System;
using System.Collections.Generic;
using System.Text;

namespace C.Protochain.Entities
{
    public class Transaction
    {
        public string Id { get; set; }
        public TransactionIn[] TransactionsIn { get; set; }
        public TransactionOut[] TransactionsOut { get; set; }
    }
}
