namespace StablingApi.Models
{
    public class MoneyAccount
    {
        public byte MoneyAccountId { get; set; }
        public string AccountName { get; set; }
        public Decimal Balance { get; set; }
    }
}