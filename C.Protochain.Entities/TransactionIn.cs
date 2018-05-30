namespace C.Protochain.Entities
{
    public class TransactionIn
    {
        public string TransactionOutId { get; set; }
        public long TransactionOutIndex { get; set; }
        public string Signature { get; set; }
    }
}