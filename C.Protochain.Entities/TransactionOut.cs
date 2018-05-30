namespace C.Protochain.Entities
{
    public class TransactionOut
    {
        public TransactionOut(string address, long amount)
        {
            Address = address;
            Amount = amount;
        }

        public string Address { get; }
        public long Amount { get; }        
    }
}