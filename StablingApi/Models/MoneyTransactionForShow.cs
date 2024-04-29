namespace StablingApi.Models
{
    public class MoneyTransactionForShow
    {
        public int MoneyTransactionId { get; set; }
        public string TrainerName { get; set; }
        public string AccountName { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
