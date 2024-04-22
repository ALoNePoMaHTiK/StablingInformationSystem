namespace StablingApi.Models
{
    public class MoneyTransaction
    {
        public int MoneyTransactionId { get; set; }
        public int TrainerId { get; set; }
        public byte MoneyAccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Amount { get; set; }
    }
}